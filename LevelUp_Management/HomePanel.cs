using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LevelUp_Management
{
    // =========================================================================
    // HomePanel — the branded landing screen shown in panelMain when the
    // application starts and whenever a panel's Back button returns the user
    // here (mainForm.showPanel(null)).
    //
    // It is read-only: the tiles count entities that are already in the
    // Program.* in-memory lists. There are no database calls, no writes and no
    // business rules here — it is a view over state that initLists() loaded at
    // startup.
    //
    // Unlike the UC panels, this screen has no Designer.cs. It has no
    // interactive controls and no event handlers to wire up, and its tiles are
    // generated from a table rather than placed by hand, so a designer file
    // would only be a second place to keep the same layout in sync.
    // =========================================================================
    public class HomePanel : UserControl
    {
        // Count-up animation for the tile numbers: each numeric tile starts at
        // 0 and eases out to its real value over CountUpMs. Purely visual -
        // the targets are the same counts tileData() computed.
        private const int CountUpMs = 600;
        private readonly List<Tuple<Label, int>> countUps = new List<Tuple<Label, int>>();
        private Timer countUpTimer;
        private Stopwatch countUpClock;

        public HomePanel()
        {
            build();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            startCountUp();
        }

        private void startCountUp()
        {
            if (countUps.Count == 0) return;
            countUpClock = Stopwatch.StartNew();
            countUpTimer = new Timer();
            countUpTimer.Interval = 30;
            countUpTimer.Tick += countUpTimer_Tick;
            countUpTimer.Start();
        }

        private void countUpTimer_Tick(object sender, EventArgs e)
        {
            double t = Math.Min(1.0, countUpClock.ElapsedMilliseconds / (double)CountUpMs);
            double eased = 1 - Math.Pow(1 - t, 3);   // ease-out cubic

            // At t = 1 the exact target is written, never a rounded value.
            foreach (Tuple<Label, int> c in countUps)
                c.Item1.Text = (t >= 1.0 ? c.Item2 : (int)Math.Round(c.Item2 * eased)).ToString();

            if (t >= 1.0)
                stopCountUp();
        }

        private void stopCountUp()
        {
            if (countUpTimer == null) return;
            countUpTimer.Stop();
            countUpTimer.Tick -= countUpTimer_Tick;
            countUpTimer.Dispose();
            countUpTimer = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                stopCountUp();
            base.Dispose(disposing);
        }

        private void build()
        {
            SuspendLayout();

            RightToLeft = RightToLeft.Yes;
            BackColor = UITheme.Canvas;
            Padding = new Padding(UITheme.dp(this, 28));

            // Added first = innermost, so the hint sits under the tiles, which
            // sit under the hero.
            Controls.Add(buildHint());
            Controls.Add(buildTiles());
            Controls.Add(buildHero());

            ResumeLayout(true);
        }

        // ------------------------------------------------------------------
        // Hero — the welcome band. Card-coloured, with top and bottom hairlines
        // (Line): Card vs Canvas alone measures only ~1.08:1 — nowhere near
        // enough to read as a distinct band on its own — so the hairlines, not
        // the fill, are what actually define its edges, same principle as
        // every band border elsewhere in UITheme.cs (buildHeader, buildActionBar).
        // ------------------------------------------------------------------
        private Panel buildHero()
        {
            Panel hero = new Panel();
            hero.Name = "hero";
            hero.Dock = DockStyle.Top;
            hero.BackColor = UITheme.Card;

            int pad = UITheme.dp(this, 32);
            int y = pad;

            Label eyebrow = new Label();
            eyebrow.AutoSize = true;
            eyebrow.Text = "LEVEL UP";
            eyebrow.Font = new Font("Trebuchet MS", 9.5F, FontStyle.Bold);
            eyebrow.ForeColor = UITheme.Accent;
            eyebrow.BackColor = Color.Transparent;
            eyebrow.RightToLeft = RightToLeft.No;   // Latin wordmark: do not mirror
            eyebrow.Location = new Point(pad, y);
            hero.Controls.Add(eyebrow);
            y += eyebrow.PreferredSize.Height + UITheme.dp(this, 10);

            Label greeting = new Label();
            greeting.AutoSize = true;
            greeting.Text = "ברוכים הבאים למערכת ניהול הסטודיו";
            greeting.Font = new Font("Trebuchet MS", 22F, FontStyle.Bold);
            greeting.ForeColor = UITheme.TextPrimary;
            greeting.BackColor = Color.Transparent;
            greeting.Location = new Point(pad, y);
            hero.Controls.Add(greeting);
            y += greeting.PreferredSize.Height + UITheme.dp(this, 6);

            Label blurb = new Label();
            blurb.AutoSize = true;
            blurb.Text = "ניהול מתאמנים, אימונים, מנויים והשכרת חדרים — במקום אחד.";
            blurb.Font = new Font("Trebuchet MS", 10.5F, FontStyle.Regular);
            blurb.ForeColor = UITheme.TextOnSidePanelMuted;
            blurb.BackColor = Color.Transparent;
            blurb.Location = new Point(pad + UITheme.dp(this, 2), y);
            hero.Controls.Add(blurb);
            y += blurb.PreferredSize.Height;

            hero.Height = y + pad;

            Panel topRule = new Panel();
            topRule.Dock = DockStyle.Top;
            topRule.Height = Math.Max(1, UITheme.dp(this, 1));
            topRule.BackColor = UITheme.Line;
            hero.Controls.Add(topRule);

            Panel bottomRule = new Panel();
            bottomRule.Dock = DockStyle.Bottom;
            bottomRule.Height = Math.Max(1, UITheme.dp(this, 1));
            bottomRule.BackColor = UITheme.Line;
            hero.Controls.Add(bottomRule);

            return hero;
        }

        // ------------------------------------------------------------------
        // Stat tiles — a live count per entity list
        // ------------------------------------------------------------------
        private Control buildTiles()
        {
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Name = "tiles";
            flow.Dock = DockStyle.Top;
            flow.BackColor = UITheme.Canvas;
            flow.FlowDirection = FlowDirection.LeftToRight;   // mirrored -> reads right to left
            flow.WrapContents = true;
            flow.AutoSize = true;
            flow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flow.Padding = new Padding(0, UITheme.dp(this, 24), 0, 0);

            // Alternating cyan/orange top accents, echoing the two brand
            // accents across the row of tiles rather than repeating one colour
            // six times over.
            List<Tuple<string, string>> data = tileData();
            for (int i = 0; i < data.Count; i++)
            {
                Color accent = (i % 2 == 0) ? UITheme.Accent : UITheme.AccentOrange;
                flow.Controls.Add(buildTile(data[i].Item1, data[i].Item2, accent));
            }

            return flow;
        }

        /// <summary>
        /// The counts shown on the dashboard. Every list is read defensively:
        /// a null list means initLists() has not run, which must show "—"
        /// rather than throw on the application's first screen.
        /// </summary>
        private List<Tuple<string, string>> tileData()
        {
            return new List<Tuple<string, string>>
            {
                Tuple.Create("מתאמנים פעילים",
                    count(Program.Trainees, t => t.getStatus() == AccountStatus.ACTIVE)),
                Tuple.Create("אימונים מתוכננים",
                    count(Program.TrainingSessions, null)),
                Tuple.Create("מנויים בתוקף",
                    count(Program.Memberships, m => m.getStatus() == MembershipStatus.ACTIVE)),
                Tuple.Create("הרשמות לאימונים",
                    count(Program.Registrations, null)),
                Tuple.Create("ממתינים בתור",
                    count(Program.WaitlistEntries, null)),
                Tuple.Create("חדרי אימון",
                    count(Program.Rooms, null))
            };
        }

        private static string count<T>(List<T> list, Func<T, bool> predicate)
        {
            if (list == null) return "—";
            return (predicate == null ? list.Count : list.Count(predicate)).ToString();
        }

        /// <summary>
        /// A rounded, flat, bordered stat card. Card vs Canvas alone measures
        /// only ~1.08:1 — far too close to read as a distinct object — so the
        /// tile is drawn as a thin Line-coloured outer shape with a Card-
        /// coloured inner face, both independently rounded to the same radius
        /// (their edges are only 1-2px apart, so a shared radius reads as one
        /// continuous curve). The coloured accent bar sits INSIDE the face,
        /// inset horizontally by the radius on both sides — verified
        /// empirically that Control.Region clips a control's own painting but
        /// NOT its children's, so a bar docked flush to the edge would show its
        /// own square corners poking past the rounded silhouette; insetting by
        /// the radius keeps it inside the flat part of the curve.
        /// </summary>
        private Panel buildTile(string caption, string value, Color accent)
        {
            const int radiusLogical = 12;
            int border = Math.Max(1, UITheme.dp(this, 1));

            Panel tile = new Panel();
            tile.BackColor = UITheme.Line;
            tile.Size = new Size(UITheme.dp(this, 172), UITheme.dp(this, 104));
            tile.Margin = new Padding(0, 0, UITheme.dp(this, 16), UITheme.dp(this, 16));

            Panel face = new Panel();
            face.Location = new Point(border, border);
            face.Size = new Size(tile.Width - border * 2, tile.Height - border * 2);
            face.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            face.BackColor = UITheme.Card;

            int barH = Math.Max(2, UITheme.dp(this, 3));
            int barInset = UITheme.dp(this, radiusLogical);
            Panel bar = new Panel();
            bar.Location = new Point(barInset, 0);
            bar.Size = new Size(Math.Max(0, face.Width - barInset * 2), barH);
            bar.BackColor = accent;

            int pad = UITheme.dp(this, 18);

            Label lblValue = new Label();
            lblValue.AutoSize = true;
            lblValue.Text = value;
            lblValue.Font = new Font("Trebuchet MS", 24F, FontStyle.Bold);
            lblValue.ForeColor = UITheme.TextPrimary;
            lblValue.BackColor = Color.Transparent;
            lblValue.Location = new Point(pad, barH + pad + UITheme.dp(this, 4));

            // Numeric tiles count up from 0 when the panel loads; "—" stays as is.
            int target;
            if (int.TryParse(value, out target))
            {
                lblValue.Text = "0";
                countUps.Add(Tuple.Create(lblValue, target));
            }

            Label lblCaption = new Label();
            lblCaption.AutoSize = true;
            lblCaption.Text = caption;
            lblCaption.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular);
            lblCaption.ForeColor = UITheme.TextMuted;
            lblCaption.BackColor = Color.Transparent;
            lblCaption.Location = new Point(pad + UITheme.dp(this, 2),
                                            lblValue.Location.Y + lblValue.PreferredSize.Height + UITheme.dp(this, 2));

            // Grow the tile (and the face along with it, via the Anchor above)
            // if a large font pushes the caption past the bottom.
            int neededFaceHeight = lblCaption.Location.Y + lblCaption.PreferredSize.Height + pad;
            int needed = neededFaceHeight + border * 2;
            if (needed > tile.Height)
            {
                tile.Height = needed;
                face.Height = neededFaceHeight;
            }

            face.Controls.Add(lblValue);
            face.Controls.Add(lblCaption);
            face.Controls.Add(bar);
            tile.Controls.Add(face);

            // Applied LAST, after both controls have their final size, so the
            // rounded silhouettes match reality rather than a stale early guess.
            UITheme.roundCorners(tile, radiusLogical);
            UITheme.roundCorners(face, Math.Max(0, radiusLogical - 2));

            return tile;
        }

        // ------------------------------------------------------------------
        // Hint
        // ------------------------------------------------------------------
        private Control buildHint()
        {
            Panel host = new Panel();
            host.Dock = DockStyle.Top;
            host.BackColor = UITheme.Canvas;

            Label hint = new Label();
            hint.AutoSize = true;
            hint.Text = "בחרו פעולה מהתפריט כדי להתחיל.";
            hint.Font = new Font("Trebuchet MS", 10F, FontStyle.Regular);
            hint.ForeColor = UITheme.TextMuted;
            hint.BackColor = Color.Transparent;
            hint.Location = new Point(UITheme.dp(this, 2), UITheme.dp(this, 14));

            host.Controls.Add(hint);
            host.Height = hint.Location.Y + hint.PreferredSize.Height + UITheme.dp(this, 8);
            return host;
        }
    }
}
