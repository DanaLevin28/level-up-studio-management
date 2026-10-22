using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LevelUp_Management
{
    // =========================================================================
    // UITheme — the single source of truth for the application's visual design
    // ("Figma Dark" — the exact dark-mode palette the studio's Figma design
    // specifies, pushed as flat as WinForms allows: zero button/panel borders,
    // solid flat fills, no bevels). This replaces the earlier light "Sport-Tech"
    // scheme; the app was still rendering that light theme because no dark
    // palette had been implemented yet, not a caching issue.
    //
    // Nothing here touches business logic, entities or stored procedures. It is
    // presentation only: colours, fonts, spacing and control re-layout.
    //
    // HOW IT IS USED
    //   mainForm      -> UITheme.applyShell(...)  from the constructor
    //   every panel   -> UITheme.applyPanel(this, "subtitle") after
    //                    InitializeComponent()
    //
    // The Designer.cs files stay free of colours and fonts on purpose, so the
    // Visual Studio form designer keeps opening them cleanly and so the palette
    // can be changed in exactly one place.
    //
    // -------------------------------------------------------------------------
    // RIGHT-TO-LEFT MIRRORING
    // mainForm sets RightToLeftLayout = true (required by CLAUDE.md). That flag
    // puts WS_EX_LAYOUTRTL on the window, which mirrors the whole layout, so:
    //
    //   * DockStyle.Left  renders on the PHYSICAL RIGHT
    //   * DockStyle.Right renders on the PHYSICAL LEFT
    //   * a control at X = 24 sits 24px from the PHYSICAL RIGHT edge
    //
    // This was verified by measuring GetWindowRect on a docked panel, not
    // assumed. It is why panelMenu is docked Left in mainForm.Designer.cs yet
    // appears on the right, and why "logical left" below always means the side
    // a Hebrew reader starts from.
    //
    // -------------------------------------------------------------------------
    // HIGH-DPI
    // Every length in this file is written in LOGICAL pixels (as if the display
    // were at 100%) and converted with dp() before use. This matters because the
    // designer's fixed sizes — a 200px-wide ComboBox, a 110px-wide Button — stay
    // at their raw values at runtime, while auto-sized text measures at the real
    // DPI. On a 200% display that mismatch makes every input look half-size next
    // to its own label, so applyPanel() scales the designer's fixed geometry up
    // itself before laying anything out. Do not hard-code a raw pixel value here.
    // =========================================================================
    public static class UITheme
    {
        // ------------------------------------------------------------------
        // Palette — light content area with a dark navigation rail.
        //   Canvas #F5F7F6 / Card #FFFFFF / Line #E2E8F0 / Accent #2D9F93
        //   Sidebar #1A1A2E (kept dark for contrast), nav text #CBD5E1.
        // TextOnAccent is white: white on #2D9F93 is fine for bold 10pt labels
        // and matches the light design.
        // ------------------------------------------------------------------

        public static readonly Color Canvas = Color.FromArgb(245, 247, 246);       // #F5F7F6
        public static readonly Color Card = Color.FromArgb(255, 255, 255);         // #FFFFFF
        public static readonly Color Line = Color.FromArgb(226, 232, 240);         // #E2E8F0
        public static readonly Color Accent = Color.FromArgb(45, 159, 147);        // #2D9F93
        public static readonly Color AccentHover = Color.FromArgb(36, 138, 128);   // #248A80
        public static readonly Color AccentTint = Color.FromArgb(240, 253, 250);   // outline-button hover fill
        public static readonly Color TextPrimary = Color.FromArgb(26, 26, 46);     // #1A1A2E
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);    // #64748B
        public static readonly Color TextOnAccent = Color.White;
        public static readonly Color Danger = Color.FromArgb(239, 68, 68);         // #EF4444
        public static readonly Color DangerHover = Color.FromArgb(220, 38, 38);    // #DC2626
        public static readonly Color DangerTint = Color.FromArgb(254, 242, 242);   // danger-button hover fill
        // Kept because HomePanel/LoginPanel still reference it; maps to the danger red.
        public static readonly Color AccentOrange = Danger;
        public static readonly Color Success =Color.FromArgb(16, 185, 129);       // #10B981

        // Grid
        public static readonly Color RowAlt = Color.FromArgb(248, 250, 251);       // #F8FAFB
        public static readonly Color GridHeaderBg = Color.FromArgb(241, 245, 249); // #F1F5F9
        public static readonly Color GridHeaderText = Color.FromArgb(71, 85, 105); // #475569
        public static readonly Color GridText = Color.FromArgb(30, 41, 59);        // #1E293B
        public static readonly Color SelectionBg = Color.FromArgb(199, 234, 230);  // #C7EAE6
        public static readonly Color FieldLabelText = Color.FromArgb(71, 85, 105); // #475569

        // Sidebar (stays dark)
        public static readonly Color SidePanel = Color.FromArgb(26, 26, 46);       // #1A1A2E
        public static readonly Color SidePanelDeep = SidePanel;                    // brand block
        public static readonly Color SurfaceHover = Color.FromArgb(45, 58, 80);    // #2D3A50 nav hover
        public static readonly Color SurfacePressed = Accent;                      // nav pressed
        public static readonly Color TextOnSidePanel = Color.FromArgb(203, 213, 225); // #CBD5E1
        public static readonly Color TextOnSidePanelMuted = Color.FromArgb(148, 163, 184);
        public static readonly Color BorderStrong = Line;

        // ------------------------------------------------------------------
        // Typography — Segoe UI.
        // ------------------------------------------------------------------
        public static readonly Font Brand = new Font("Segoe UI", 21F, FontStyle.Bold);
        public static readonly Font BrandSub = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font PanelTitle = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static readonly Font PanelSubtitle = new Font("Segoe UI", 9.75F, FontStyle.Regular);
        public static readonly Font SidebarCaption = new Font("Segoe UI", 8.75F, FontStyle.Bold);
        public static readonly Font NavButton = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font ActionButton = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font SecondaryButton = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font FieldLabel = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font Input = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font GridHeader = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font GridCell = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font NavButtonRegular = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font FormTitleFont = new Font("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font FormHintFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);

        // ------------------------------------------------------------------
        // Spacing scale — logical pixels, always passed through dp()
        // ------------------------------------------------------------------
        public const int Gutter = 24;        // band edge to content
        public const int RowGap = 14;        // vertical gap between form rows
        public const int MinFieldGap = 10;   // smallest gap between two controls in a row
        public const int BandPadTop = 20;
        public const int BandPadBottom = 22;
        public const int ButtonHeight = 40;
        public const int ButtonGap = 12;
        public const int NavButtonHeight = 52;
        public const int NavGap = 10;
        public const int SidebarWidth = 272;
        public const int SidebarPad = 20;
        public const int GridRowHeight = 40;
        public const int GridHeaderHeight = 44;

        // ------------------------------------------------------------------
        // dp() — logical pixels to device pixels for the display the control is
        // actually on.
        // ------------------------------------------------------------------
        // Used for the theme's OWN spacing only. The designer's geometry must not
        // be scaled here: every Designer.cs declares AutoScaleDimensions (8F, 16F)
        // with AutoScaleMode.Font, so WinForms has already scaled those controls
        // by the time InitializeComponent() returns. Scaling them again here
        // compounded the two factors and turned a 110px button into a 358px one.
        private static float scaleOf(Control c)
        {
            return Math.Max(1f, c.DeviceDpi / 96f);
        }

        private static int dp(float scale, int logical)
        {
            return (int)Math.Round(logical * scale);
        }

        /// <summary>Logical pixels to device pixels for the display <paramref name="c"/> is on.</summary>
        public static int dp(Control c, int logical)
        {
            return dp(scaleOf(c), logical);
        }

        /// <summary>
        /// Clips a control to a rounded-rectangle silhouette via Region — the
        /// standard WinForms technique for a flat, rounded "card" corner without
        /// owner-drawing the whole control. Call this AFTER the control's final
        /// Size is set; there is no live resize hook, because every caller today
        /// builds a fixed-size control once and never resizes it afterward, so a
        /// one-time Region assignment is simpler and safer than wiring a
        /// persistent Resize handler for no real benefit.
        ///
        /// IMPORTANT — verified empirically (captured actual pixels via
        /// Control.DrawToBitmap, not assumed): Control.Region clips ONLY that
        /// control's OWN background painting. It does NOT clip that control's
        /// CHILD controls — they are separate windows layered on top and paint
        /// their full square bounds regardless of the parent's Region. A child
        /// docked flush to an edge (e.g. a full-width accent bar at the very
        /// top) will show its own square corners poking past the rounded
        /// silhouette. Inset any such child by at least <paramref name="radiusLogical"/>
        /// from the edges the rounding affects to avoid that.
        /// </summary>
        public static void roundCorners(Control c, int radiusLogical)
        {
            int r = dp(c, radiusLogical);
            if (r <= 0 || c.Width <= 0 || c.Height <= 0) return;

            using (GraphicsPath path = new GraphicsPath())
            {
                Rectangle rc = new Rectangle(0, 0, c.Width, c.Height);
                path.AddArc(rc.X, rc.Y, r, r, 180, 90);
                path.AddArc(rc.Right - r, rc.Y, r, r, 270, 90);
                path.AddArc(rc.Right - r, rc.Bottom - r, r, r, 0, 90);
                path.AddArc(rc.X, rc.Bottom - r, r, r, 90, 90);
                path.CloseFigure();
                c.Region = new Region(path);
            }
        }

        // ==================================================================
        // SHELL — mainForm
        // ==================================================================

        /// <summary>
        /// Styles the application shell: the dark navigation rail (brand block,
        /// section caption, nav buttons) and the content canvas.
        /// </summary>
        public static void applyShell(Form form, Panel menu, Panel main, Label menuCaption)
        {
            float s = scaleOf(form);
            form.SuspendLayout();

            form.BackColor = Canvas;
            form.ForeColor = TextPrimary;

            main.BackColor = Canvas;
            main.Padding = new Padding(0);

            menu.BackColor = SidePanel;
            menu.Width = dp(s, SidebarWidth);
            menu.Padding = new Padding(0);

            int pad = dp(s, SidebarPad);
            int navWidth = menu.Width - pad * 2;

            // Take the designer's nav buttons out so they can be re-hosted in a
            // flow layout, where spacing comes from Margin rather than from
            // hand-computed Y coordinates.
            List<Button> navButtons = menu.Controls.OfType<Button>()
                                          .OrderBy(b => b.Top)
                                          .ToList();
            FlowLayoutPanel navFlow = new FlowLayoutPanel();
            navFlow.Name = "navFlow";
            navFlow.Dock = DockStyle.Fill;
            navFlow.FlowDirection = FlowDirection.TopDown;
            navFlow.WrapContents = false;
            navFlow.BackColor = SidePanel;
            navFlow.Padding = new Padding(pad, dp(s, 4), pad, pad);

            foreach (Button b in navButtons)
            {
                menu.Controls.Remove(b);
                styleNavButton(b, navWidth, dp(s, NavButtonHeight));
                b.Margin = new Padding(0, 0, 0, dp(s, NavGap));
                navFlow.Controls.Add(b);
            }

            // --- section caption above the buttons -------------------------
            Panel captionBand = new Panel();
            captionBand.Name = "captionBand";
            captionBand.Dock = DockStyle.Top;
            captionBand.BackColor = SidePanel;
            captionBand.Height = dp(s, 40);
            if (menuCaption != null)
            {
                menu.Controls.Remove(menuCaption);
                menuCaption.AutoSize = true;
                menuCaption.Font = SidebarCaption;
                menuCaption.ForeColor = TextOnSidePanelMuted;
                menuCaption.BackColor = Color.Transparent;
                menuCaption.Location = new Point(pad, dp(s, 14));
                captionBand.Controls.Add(menuCaption);
                captionBand.Height = menuCaption.Location.Y + menuCaption.PreferredSize.Height + dp(s, 8);
            }

            // --- brand block -----------------------------------------------
            Panel brand = new Panel();
            brand.Name = "brandBlock";
            brand.Dock = DockStyle.Top;
            brand.BackColor = SidePanelDeep;
            brand.Width = menu.Width;

            Label lblBrand = new Label();
            lblBrand.Name = "lblBrand";
            lblBrand.AutoSize = true;
            lblBrand.Text = "LEVEL UP";
            lblBrand.Font = Brand;
            lblBrand.ForeColor = Accent;
            lblBrand.BackColor = Color.Transparent;
            // A Latin wordmark: keep it LTR so mirroring cannot reverse the words.
            lblBrand.RightToLeft = RightToLeft.No;
            lblBrand.Location = new Point(pad, dp(s, 22));

            Label lblBrandSub = new Label();
            lblBrandSub.Name = "lblBrandSub";
            lblBrandSub.AutoSize = true;
            lblBrandSub.Text = "מערכת ניהול הסטודיו";
            lblBrandSub.Font = BrandSub;
            lblBrandSub.ForeColor = TextOnSidePanelMuted;
            lblBrandSub.BackColor = Color.Transparent;
            lblBrandSub.Location = new Point(pad + dp(s, 2),
                                             lblBrand.Location.Y + lblBrand.PreferredSize.Height + dp(s, 2));

            // Height derives from the measured text, so the block cannot clip
            // its own wordmark at any DPI or font scale.
            brand.Height = lblBrandSub.Location.Y + lblBrandSub.PreferredSize.Height + dp(s, 20);

            // A 3px cyan rule under the wordmark — a plain fill, no text sitting
            // on it, so the "Cyan needs dark text on top of it" rule doesn't
            // apply here.
            Panel brandRule = new Panel();
            brandRule.Dock = DockStyle.Bottom;
            brandRule.Height = Math.Max(2, dp(s, 3));
            brandRule.BackColor = Accent;

            brand.Controls.Add(lblBrand);
            brand.Controls.Add(lblBrandSub);
            brand.Controls.Add(brandRule);

            // --- studio logo -------------------------------------------------
            PictureBox logo = new PictureBox();
            logo.Dock = DockStyle.Top;
            logo.Height = dp(s, 80);
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.BackColor = Color.FromArgb(26, 26, 46);
            logo.Padding = new Padding(dp(s, 12));
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo_transparent3.png");
            if (File.Exists(logoPath))
                logo.Image = Image.FromFile(logoPath);

            // Dock priority resolves from the HIGHEST z-index down and
            // Controls.Add appends, so the control added LAST becomes the
            // outermost band: brand on top, then the caption, then the buttons.
            menu.Controls.Add(navFlow);
            menu.Controls.Add(captionBand);
            menu.Controls.Add(brand);
            menu.Controls.Add(logo);

            // Both the rail and the content area are pure white, so nothing
            // separates them without an explicit hairline. Docked Right because
            // mirroring puts that on the PHYSICAL LEFT — the edge that actually
            // faces panelMain.
            Panel railEdge = new Panel();
            railEdge.Name = "railEdge";
            railEdge.Dock = DockStyle.Right;
            railEdge.Width = Math.Max(1, dp(s, 1));
            railEdge.BackColor = BorderStrong;
            menu.Controls.Add(railEdge);

            form.ResumeLayout(true);
        }

        // ==================================================================
        // PANELS
        // ==================================================================

        /// <summary>
        /// Restyles and re-lays-out a UC panel that the designer built with
        /// absolute coordinates, turning it into docked bands:
        ///
        ///     header   (Top)     title + subtitle on a white card
        ///     filters  (Top)     controls the designer placed above the grid
        ///     grid     (Fill)    stretches with the window — this is what fixes
        ///                        the clipping the fixed 860px grids suffered
        ///     form     (Bottom)  the edit fields
        ///     actions  (Bottom)  the bottom button row
        ///
        /// Existing controls are moved, never recreated, so every event handler
        /// the designer wired up stays connected.
        /// </summary>
        public static void applyPanel(UserControl panel, string subtitle)
        {
            float s = scaleOf(panel);

            panel.SuspendLayout();
            panel.RightToLeft = RightToLeft.Yes;   // CLAUDE.md: explicit on every UserControl
            panel.BackColor = Canvas;
            panel.ForeColor = TextPrimary;
            panel.Padding = new Padding(0);

            // The designer's geometry is already at the right scale — WinForms
            // applied it when InitializeComponent() returned. Only the theme's own
            // spacing needs dp().
            List<Control> children = panel.Controls.Cast<Control>().ToList();

            Label title = children.OfType<Label>().FirstOrDefault(l => l.Name == "lblTitle");
            DataGridView grid = children.OfType<DataGridView>().FirstOrDefault();
            int gridTop = grid != null ? grid.Top : int.MaxValue;

            // The bottom button row = every Button sharing the lowest Y band.
            List<Button> buttons = children.OfType<Button>().ToList();
            List<Control> actionControls = new List<Control>();
            if (buttons.Count > 0)
            {
                int actionRowY = buttons.Max(b => b.Top);
                actionControls = buttons.Where(b => b.Top >= actionRowY - dp(s, 10))
                                        .OrderBy(b => b.Left)
                                        .Cast<Control>()
                                        .ToList();
            }

            List<Control> rest = children
                .Where(c => c != title && c != grid && !actionControls.Contains(c))
                .ToList();

            List<Control> aboveGrid = rest.Where(c => c.Top < gridTop).ToList();
            List<Control> belowGrid = rest.Where(c => c.Top >= gridTop).ToList();

            // Style the leaves before measuring anything, so the layout below
            // works from final sizes.
            foreach (Control c in rest) styleFieldControl(c, s);
            foreach (Control c in actionControls) styleActionButton((Button)c, s);
            if (grid != null) styleGrid(grid, s);

            Panel header = buildHeader(panel, title, subtitle, s);
            Panel filterBand = buildFormBand(panel, aboveGrid, true, s);
            Panel formBand = buildFormBand(panel, belowGrid, false, s);
            Panel actionBar = buildActionBar(panel, actionControls, s);
            Panel gridHost = buildGridHost(panel, grid, s);

            // Added first = innermost. This order yields, top to bottom:
            //     header / filters / grid (fill) / form / actions
            panel.Controls.Add(gridHost);
            panel.Controls.Add(filterBand);
            panel.Controls.Add(formBand);
            panel.Controls.Add(header);
            panel.Controls.Add(actionBar);

            // Step 2: never let the form band be shorter than its lowest control.
            if (formBand.Visible)
            {
                int maxBottom = 0;
                foreach (Control c in formBand.Controls)
                {
                    if (c is Panel && c.Dock != DockStyle.None) continue; // skip the rule
                    maxBottom = Math.Max(maxBottom, c.Bottom);
                }
                int needed = maxBottom + dp(s, BandPadBottom + 10);
                if (needed > formBand.Height) formBand.Height = needed;
                formBand.MinimumSize = new Size(0, Math.Max(needed, formBand.Height));
            }

            panel.ResumeLayout(true);

            // TEMP-DIAG (remove after layout bug is found)
            logLayout("applyPanel end (panel not yet parented)", panel, header, filterBand, formBand, actionBar, gridHost);
            panel.Load += delegate
            {
                logLayout("panel.Load (after parenting)", panel, header, filterBand, formBand, actionBar, gridHost);
            };
        }

        // TEMP-DIAG: appends to layout-diag.txt next to the exe, and to the debug output.
        private static void logLayout(string stage, UserControl panel, Panel header,
            Panel filterBand, Panel formBand, Panel actionBar, Panel gridHost)
        {
            string line = string.Format(
                "[{0:HH:mm:ss}] {1} | {2} | DeviceDpi={3} scaleOf={4:0.00} | panel.Size={5} panel.Parent={6} parent.Height={7} | " +
                "CurrentAutoScaleDims={8} Font={9} | header.H={10} filter.H={11} form.H={12} actions.H={13} grid.H={14} (grid.Top={15}, form.Top={16}) | " +
                "form.controls.bottom={17}",
                DateTime.Now, stage, panel.GetType().Name, panel.DeviceDpi, scaleOf(panel), panel.Size,
                panel.Parent == null ? "null" : panel.Parent.Name, panel.Parent == null ? -1 : panel.Parent.Height,
                panel.CurrentAutoScaleDimensions, panel.Font,
                header.Height, filterBand.Height, formBand.Height, actionBar.Height, gridHost.Height,
                gridHost.Top, formBand.Top,
                formBand.Controls.Cast<Control>().Select(c => c.Bottom).DefaultIfEmpty(0).Max());
            line += Environment.NewLine + "    formBand.W=" + formBand.Width + " controls: " + string.Join("; ",
                formBand.Controls.Cast<Control>().Where(c => !(c is Panel)).Select(c =>
                    c.GetType().Name + ":" + c.Name + " " + c.Bounds + " autosize=" + c.AutoSize));
            System.Diagnostics.Debug.WriteLine(line);
            try
            {
                System.IO.File.AppendAllText(
                    System.IO.Path.Combine(AppContext.BaseDirectory, "layout-diag.txt"), line + Environment.NewLine);
            }
            catch { }
        }

        public static void applyPanel(UserControl panel)
        {
            applyPanel(panel, null);
        }

        // ------------------------------------------------------------------
        // Band builders
        // ------------------------------------------------------------------

        private static Panel buildHeader(UserControl panel, Label title, string subtitle, float s)
        {
            Panel header = new Panel();
            header.Name = "bandHeader";
            header.Dock = DockStyle.Top;
            header.BackColor = Card;
            header.Width = panel.Width;

            int pad = dp(s, Gutter);
            int y = dp(s, 16);

            if (title != null)
            {
                panel.Controls.Remove(title);
                title.AutoSize = true;
                title.Font = PanelTitle;
                title.ForeColor = TextPrimary;
                title.BackColor = Color.Transparent;
                title.Location = new Point(pad, y);
                header.Controls.Add(title);
                y = title.Location.Y + title.PreferredSize.Height;
            }

            if (!string.IsNullOrEmpty(subtitle))
            {
                Label sub = new Label();
                sub.Name = "lblSubtitle";
                sub.AutoSize = true;
                sub.Text = subtitle;
                sub.Font = PanelSubtitle;
                sub.ForeColor = TextMuted;
                sub.BackColor = Color.Transparent;
                sub.Location = new Point(pad + dp(s, 2), y + dp(s, 2));
                header.Controls.Add(sub);
                y = sub.Location.Y + sub.PreferredSize.Height;
            }

            // Derived from measured text so the title can never be clipped.
            header.Height = y + dp(s, 16);

            Panel rule = new Panel();
            rule.Dock = DockStyle.Bottom;
            rule.Height = Math.Max(1, dp(s, 1));
            rule.BackColor = BorderStrong;
            header.Controls.Add(rule);

            return header;
        }

        private static Panel buildGridHost(UserControl panel, DataGridView grid, float s)
        {
            Panel host = new Panel();
            host.Name = "bandGrid";
            host.Dock = DockStyle.Fill;
            host.BackColor = Canvas;
            host.Padding = new Padding(dp(s, Gutter), dp(s, 18), dp(s, Gutter), dp(s, 18));

            if (grid != null)
            {
                // A 1px frame painted by the parent's background — crisper than
                // DataGridView's own border styles.
                Panel frame = new Panel();
                frame.Dock = DockStyle.Fill;
                frame.BackColor = BorderStrong;
                frame.Padding = new Padding(Math.Max(1, dp(s, 1)));

                panel.Controls.Remove(grid);
                grid.Dock = DockStyle.Fill;
                frame.Controls.Add(grid);
                host.Controls.Add(frame);
            }

            return host;
        }

        private static Panel buildActionBar(UserControl panel, List<Control> actions, float s)
        {
            Panel bar = new Panel();
            bar.Name = "bandActions";
            bar.Dock = DockStyle.Bottom;
            bar.BackColor = Card;
            bar.Width = panel.Width;

            Panel rule = new Panel();
            rule.Dock = DockStyle.Top;
            rule.Height = Math.Max(1, dp(s, 1));
            rule.BackColor = BorderStrong;
            bar.Controls.Add(rule);

            if (actions.Count == 0)
            {
                bar.Height = 0;
                bar.Visible = false;
                return bar;
            }

            int btnH = dp(s, ButtonHeight);
            foreach (Control c in actions)
            {
                panel.Controls.Remove(c);
                c.Height = btnH;
                bar.Controls.Add(c);
            }

            // "Back" goes to the far end, away from the primary actions. Under
            // mirroring the primary actions land where a Hebrew reader starts and
            // Back lands opposite them.
            new ActionFlow(bar, actions, actions.FirstOrDefault(c => c.Name == "btnBack"), s).reflow();
            return bar;
        }

        /// <summary>
        /// Lays out the bottom button row, re-running on every resize. Back sits
        /// at the opposite end while the row fits on one line; once it does not,
        /// every button simply flows and wraps instead.
        /// </summary>
        private sealed class ActionFlow
        {
            private readonly Panel bar;
            private readonly List<Control> actions;
            private readonly Control back;
            private readonly int pad, gap, rowGap, btnH, padY;
            private bool busy;

            public ActionFlow(Panel bar, List<Control> actions, Control back, float s)
            {
                this.bar = bar;
                this.actions = actions;
                this.back = back;
                pad = dp(s, Gutter);
                gap = dp(s, ButtonGap);
                rowGap = dp(s, 10);
                padY = dp(s, 16);
                btnH = dp(s, ButtonHeight);
                bar.SizeChanged += (sender, e) => reflow();
            }

            public void reflow()
            {
                if (busy) return;
                busy = true;
                try
                {
                    // See BandFlow.reflow — settle rather than swallow the resize
                    // our own height change provokes.
                    for (int guard = 0; guard < 4; guard++)
                    {
                        int startWidth = bar.ClientSize.Width;
                        layoutOnce();
                        if (bar.ClientSize.Width == startWidth) break;
                    }
                }
                finally { busy = false; }
            }

            private void layoutOnce()
            {
                    int available = bar.ClientSize.Width - pad * 2;
                    if (available <= 0) return;

                    List<Control> primary = actions.Where(c => c != back).ToList();
                    int primaryWidth = primary.Sum(c => c.Width) + gap * Math.Max(0, primary.Count - 1);
                    int backWidth = back == null ? 0 : back.Width + gap * 2;

                    if (primaryWidth + backWidth <= available)
                    {
                        int x = pad;
                        foreach (Control c in primary)
                        {
                            c.Location = new Point(x, padY);
                            x += c.Width + gap;
                        }
                        if (back != null)
                            back.Location = new Point(bar.ClientSize.Width - pad - back.Width, padY);

                        setHeight(btnH + padY * 2);
                    }
                    else
                    {
                        int x = pad, y = padY, rows = 1;
                        foreach (Control c in actions)
                        {
                            if (x > pad && x + c.Width > pad + available)
                            {
                                x = pad;
                                y += btnH + rowGap;
                                rows++;
                            }
                            c.Location = new Point(x, y);
                            x += c.Width + gap;
                        }
                        setHeight(rows * btnH + (rows - 1) * rowGap + padY * 2);
                    }
            }

            private void setHeight(int wanted)
            {
                if (bar.Height != wanted) bar.Height = wanted;
            }
        }

        /// <summary>
        /// Rebuilds a set of absolutely-positioned designer controls into a band
        /// that reflows to the width it is actually given.
        ///
        /// Fixed coordinates cannot survive here: the designer laid these panels
        /// out for a 900px window at 100% scaling, so on a 200% display the same
        /// layout needs 1800px and the window simply is not that wide. Instead the
        /// controls are grouped into label+input units and flowed, wrapping onto a
        /// new line when the next unit will not fit. The band then reports the
        /// height it actually needs, and the grid above it absorbs the difference.
        /// </summary>
        private static Panel buildFormBand(UserControl panel, List<Control> controls, bool isFilterBand, float s)
        {
            Panel band = new Panel();
            band.Name = isFilterBand ? "bandFilters" : "bandForm";
            band.Dock = isFilterBand ? DockStyle.Top : DockStyle.Bottom;
            band.BackColor = Card;
            band.Width = panel.Width;

            if (controls.Count == 0)
            {
                band.Height = 0;
                band.Visible = false;
                return band;
            }

            Panel rule = new Panel();
            rule.Dock = isFilterBand ? DockStyle.Bottom : DockStyle.Top;
            rule.Height = Math.Max(1, dp(s, 1));
            rule.BackColor = BorderStrong;
            band.Controls.Add(rule);

            // Subtle 1px card outline.
            band.Paint += (sender, e) =>
            {
                Panel p = (Panel)sender;
                using (Pen pen = new Pen(Line, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            };

            // Read the designer's controls back in visual order (row by row, then
            // across), which is the order a reader expects them to flow in.
            List<Control> ordered = new List<Control>();
            foreach (List<Control> row in groupIntoRows(controls, dp(s, 10)))
                ordered.AddRange(row.OrderBy(c => c.Left));

            foreach (Control c in ordered)
            {
                panel.Controls.Remove(c);
                band.Controls.Add(c);
            }

            // Edit-form band gets a title + hint; the fields start below them.
            int startY = 0;
            if (!isFilterBand)
            {
                Label formTitle = new Label();
                formTitle.AutoSize = true;
                formTitle.Text = "פרטי הרשומה";
                formTitle.Font = FormTitleFont;
                formTitle.ForeColor = Accent;
                formTitle.BackColor = Color.Transparent;
                formTitle.Location = new Point(dp(s, Gutter), dp(s, 12));
                band.Controls.Add(formTitle);

                Label formHint = new Label();
                formHint.AutoSize = true;
                formHint.Text = "בחירת שורה בטבלה תציג כאן את פרטיה";
                formHint.Font = FormHintFont;
                formHint.ForeColor = TextMuted;
                formHint.BackColor = Color.Transparent;
                formHint.Location = new Point(dp(s, Gutter),
                    formTitle.Location.Y + formTitle.PreferredSize.Height + dp(s, 2));
                band.Controls.Add(formHint);

                startY = formHint.Location.Y + formHint.PreferredSize.Height + dp(s, 14);
            }

            new BandFlow(band, buildUnits(ordered), s, startY).reflow();
            return band;
        }

        /// <summary>
        /// Groups the band's controls into units that must never be split across
        /// a wrap: a Label plus the control it labels, or a lone control.
        /// </summary>
        private static List<List<Control>> buildUnits(List<Control> ordered)
        {
            List<List<Control>> units = new List<List<Control>>();
            for (int i = 0; i < ordered.Count; i++)
            {
                List<Control> unit = new List<Control>();
                unit.Add(ordered[i]);
                if (ordered[i] is Label && i + 1 < ordered.Count && !(ordered[i + 1] is Label))
                {
                    unit.Add(ordered[i + 1]);
                    i++;
                }
                units.Add(unit);
            }
            return units;
        }

        private static List<List<Control>> groupIntoRows(List<Control> controls, int tolerance)
        {
            List<List<Control>> rows = new List<List<Control>>();
            foreach (Control c in controls.OrderBy(c => c.Top))
            {
                List<Control> row = rows.FirstOrDefault(r => Math.Abs(r[0].Top - c.Top) <= tolerance);
                if (row == null)
                {
                    row = new List<Control>();
                    rows.Add(row);
                }
                row.Add(c);
            }
            return rows;
        }

        /// <summary>
        /// Flows a band's units left-to-right with wrapping, and keeps doing so
        /// whenever the band is resized. Anchored to the band's lifetime by the
        /// SizeChanged subscription.
        /// </summary>
        private sealed class BandFlow
        {
            private readonly Panel band;
            private readonly List<List<Control>> units;
            private readonly int pad, innerGap, unitGap, rowGap, padTop, padBottom, minInput;
            private bool busy;

            public BandFlow(Panel band, List<List<Control>> units, float s, int startY)
            {
                this.band = band;
                this.units = units;
                pad = dp(s, Gutter);
                innerGap = dp(s, MinFieldGap);
                unitGap = dp(s, 44);
                rowGap = dp(s, RowGap);
                padTop = Math.Max(dp(s, BandPadTop), startY);
                padBottom = dp(s, BandPadBottom);
                minInput = dp(s, 90);
                band.SizeChanged += (sender, e) => reflow();
                // Label/input sizes can change after construction (DPI, font,
                // AutoSize), so re-measure once the band is really on screen.
                band.HandleCreated += (sender, e) => reflow();
                band.VisibleChanged += (sender, e) => reflow();
                band.ParentChanged += (sender, e) => reflow();
            }

            public void reflow()
            {
                if (busy) return;
                busy = true;
                try
                {
                    // Changing our own Height re-lays-out the parent, which can
                    // change our Width again. Settle in a bounded loop rather than
                    // let the re-entrancy guard swallow that second resize.
                    for (int guard = 0; guard < 4; guard++)
                    {
                        int startWidth = band.ClientSize.Width;
                        layoutOnce();
                        if (band.ClientSize.Width == startWidth) break;
                    }
                }
                finally { busy = false; }
            }

            private void layoutOnce()
            {
                    int available = band.ClientSize.Width - pad * 2;
                    if (available <= 0) return;

                    int x = pad;
                    int y = padTop;
                    int rowHeight = 0;

                    foreach (List<Control> unit in units)
                    {
                        int unitWidth = unit.Sum(c => c.Width) + innerGap * (unit.Count - 1);

                        // A unit wider than the band would overflow, so give the
                        // input the space that is left rather than let it clip.
                        if (unitWidth > available && unit.Count > 1)
                        {
                            Control input = unit[unit.Count - 1];
                            int room = available - (unitWidth - input.Width);
                            if (room > minInput) input.Width = room;
                            unitWidth = unit.Sum(c => c.Width) + innerGap * (unit.Count - 1);
                        }

                        if (x > pad && x + unitWidth > pad + available)
                        {
                            x = pad;
                            y += rowHeight + rowGap;
                            rowHeight = 0;
                        }

                        int unitHeight = unit.Max(c => c is Label ? Math.Max(c.Height, c.PreferredSize.Height) : c.Height);
                        int cx = x;
                        foreach (Control c in unit)
                        {
                            // Centre each control on its unit so a label shares a
                            // baseline with the taller input beside it.
                            c.Location = new Point(cx, y + (unitHeight - c.Height) / 2);
                            cx += c.Width + innerGap;
                        }

                        if (unitHeight > rowHeight) rowHeight = unitHeight;
                        x += unitWidth + unitGap;
                    }

                    // Never shorter than the lowest control actually placed, so a
                    // mis-measured row cannot clip the fields.
                    int lowest = units.SelectMany(u => u).Select(c => c.Bottom).DefaultIfEmpty(0).Max();
                    int wanted = Math.Max(y + rowHeight, lowest) + padBottom;
                    if (band.Height != wanted) band.Height = wanted;
            }
        }

        // ------------------------------------------------------------------
        // Leaf control styling
        // ------------------------------------------------------------------

        // Switches the keyboard layout to Hebrew when a field gets focus.
        private static void forceHebrewInput(object sender, EventArgs e)
        {
            try
            {
                InputLanguage.CurrentInputLanguage =
                    InputLanguage.FromCulture(new System.Globalization.CultureInfo("he-IL"));
            }
            catch { }
        }

        private static void styleFieldControl(Control c, float s)
        {
            if (c is Label)
            {
                Label l = (Label)c;
                l.AutoSize = true;
                l.Font = FieldLabel;
                l.ForeColor = FieldLabelText;
                l.BackColor = Color.Transparent;
            }
            else if (c is TextBox)
            {
                TextBox t = (TextBox)c;
                t.Font = Input;
                t.RightToLeft = RightToLeft.Yes;
                t.ImeMode = ImeMode.On;
                t.GotFocus += forceHebrewInput;
                t.BorderStyle = BorderStyle.FixedSingle;
                t.BackColor = Card;
                t.ForeColor = GridText;
            }
            else if (c is ComboBox)
            {
                ComboBox cb = (ComboBox)c;
                cb.Font = Input;
                cb.RightToLeft = RightToLeft.Yes;
                cb.ImeMode = ImeMode.On;
                cb.GotFocus += forceHebrewInput;
                cb.FlatStyle = FlatStyle.Flat;
                cb.BackColor = Card;
                cb.ForeColor = GridText;
            }
            else if (c is DateTimePicker)
            {
                DateTimePicker d = (DateTimePicker)c;
                d.Font = Input;
                d.CalendarMonthBackground = Card;
                d.CalendarTitleBackColor = GridHeaderBg;
                // GridHeaderBg = Canvas, the app's darkest surface — a text-
                // bearing dark BAND, not a Cyan/Orange fill, so this takes
                // TextPrimary (light text on dark), not TextOnAccent (which is
                // deliberately the OPPOSITE — a dark foreground for the
                // mid-luminance Cyan/Orange fills). Mixing these two up is
                // exactly the "grid header text = 1.00:1" bug the WCAG audit
                // caught in an earlier revision of this file.
                d.CalendarTitleForeColor = TextPrimary;
                d.CalendarForeColor = TextPrimary;
            }
            else if (c is CheckBox)
            {
                CheckBox ch = (CheckBox)c;
                ch.AutoSize = true;
                ch.Font = Input;
                ch.ForeColor = TextPrimary;
                ch.BackColor = Color.Transparent;
                ch.Padding = new Padding(dp(s, 4), dp(s, 2), dp(s, 8), dp(s, 2));
            }
            else if (c is Button)
            {
                // A button sitting among the fields (e.g. "הפקת דוח") is the
                // primary action of that band.
                stylePrimaryButton((Button)c, s);
            }
        }

        private static void styleActionButton(Button b, float s)
        {
            switch (b.Name)
            {
                case "btnSave":
                case "btnRegister":
                case "btnGenerate":
                    stylePrimaryButton(b, s);
                    break;
                case "btnDelete":
                    styleDangerButton(b, s);
                    break;
                case "btnBack":
                    styleGhostButton(b, s);
                    break;
                default:
                    // btnUpdate, btnPayments, btnManageBookings — secondary
                    // actions that still need to read as real buttons.
                    styleOutlineButton(b, s);
                    break;
            }
        }

        private static void resetButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.UseVisualStyleBackColor = false;
            b.Font = ActionButton;
            b.Cursor = Cursors.Hand;
            b.TextAlign = ContentAlignment.MiddleCenter;
        }

        // Flat, single-fill CTA: the SAME literal Cyan in every interaction
        // state. See the file-level note on why there is no lightened/darkened
        // hover shade here — WinForms Button has no per-state ForeColor, and
        // Canvas-on-Cyan is the one foreground/fill pairing that stays legible,
        // so changing the fill on hover risks the text failing contrast in that
        // state. Zero border (resetButton already set BorderSize = 0).
        public static void stylePrimaryButton(Button b, float s)
        {
            resetButton(b);
            b.BackColor = Accent;
            b.ForeColor = TextOnAccent;
            b.FlatAppearance.MouseOverBackColor = AccentHover;
            b.FlatAppearance.MouseDownBackColor = AccentHover;
        }

        // Filled/outlined buttons whose text colour also changes on hover.
        // FlatAppearance has no per-state ForeColor, so hover is done with
        // MouseEnter/MouseLeave; the handlers are attached once per button.
        private static void styleOutlined(Button b, Color rest, Color hoverText, Color hoverFill)
        {
            resetButton(b);
            b.Font = SecondaryButton;
            b.BackColor = Card;
            b.ForeColor = rest;
            b.FlatAppearance.BorderColor = rest;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = hoverFill;
            b.FlatAppearance.MouseDownBackColor = hoverFill;

            if (b.Tag as string == "outlinedHover") return;
            b.Tag = "outlinedHover";
            b.MouseEnter += (sender, e) => ((Button)sender).ForeColor = hoverText;
            b.MouseLeave += (sender, e) => ((Button)sender).ForeColor = rest;
        }

        // Secondary actions (Update, Payments, Manage Bookings, Back): teal outline.
        public static void styleOutlineButton(Button b, float s)
        {
            styleOutlined(b, Accent, AccentHover, AccentTint);
        }

        public static void styleGhostButton(Button b, float s)
        {
            styleOutlineButton(b, s);
        }

        // Delete: flat, single-fill Orange in every state — the palette's
        // secondary accent, used here for its one clear, meaningful job
        // (destructive/caution), always visible rather than hover-only. Same
        // reasoning as stylePrimaryButton for why the fill never changes: no
        // per-state ForeColor exists, and Canvas-on-Orange is the pairing that
        // stays legible (7.63:1; TextPrimary-on-Orange only manages 2.03:1).
        public static void styleDangerButton(Button b, float s)
        {
            styleOutlined(b, Danger, DangerHover, DangerTint);
        }

        public static void styleNavButton(Button b, int width, int height)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.UseVisualStyleBackColor = false;
            b.BackColor = SidePanel;
            b.ForeColor = TextOnSidePanel;
            b.Font = NavButton;
            b.Cursor = Cursors.Hand;
            b.Size = new Size(width, height);
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.FlatAppearance.MouseOverBackColor = SurfaceHover;
            // Pressed stays a light grey. Using Accent here would drop a
            // near-black fill under dark nav text and make the label vanish
            // for as long as the button is held.
            b.FlatAppearance.MouseDownBackColor = SurfacePressed;
        }

        /// <summary>
        /// Highlights the active nav button and resets the others. Call from
        /// mainForm.showPanel() after switching panels. Buttons are searched
        /// recursively because applyShell hosts them inside a flow panel.
        /// </summary>
        public static void highlightActiveNavButton(Control menuPanel, Button activeButton)
        {
            foreach (Control c in menuPanel.Controls)
            {
                if (c is Button btn)
                {
                    bool active = btn == activeButton;
                    btn.BackColor = active ? Accent : SidePanel;
                    btn.ForeColor = active ? Color.White : TextOnSidePanel;
                    btn.Font = active ? NavButton : NavButtonRegular;
                }
                else if (c.HasChildren)
                {
                    highlightActiveNavButton(c, activeButton);
                }
            }
        }

        // ------------------------------------------------------------------
        // DataGridView
        // ------------------------------------------------------------------

        public static void styleGrid(DataGridView g, float s)
        {
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Card;
            g.GridColor = Line;
            g.EnableHeadersVisualStyles = false;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // AllCells row auto-sizing would override RowTemplate.Height and give
            // back the cramped 24px rows.
            g.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            g.RowTemplate.Height = dp(s, GridRowHeight);
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            g.ColumnHeadersHeight = dp(s, GridHeaderHeight);

            Padding cellPad = new Padding(dp(s, 8), dp(s, 4), dp(s, 8), dp(s, 4));

            // GridHeaderBg = Canvas, a dark BAND that holds text directly, so it
            // takes TextPrimary (light-on-dark) — NOT TextOnAccent, which is
            // deliberately the opposite: a dark foreground for the mid-
            // luminance Cyan/Orange button fills. See the file-level note.
            g.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBg;
            g.ColumnHeadersDefaultCellStyle.ForeColor = GridHeaderText;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeaderBg;
            g.ColumnHeadersDefaultCellStyle.SelectionForeColor = GridHeaderText;
            g.ColumnHeadersDefaultCellStyle.Font = GridHeader;
            g.ColumnHeadersDefaultCellStyle.Padding = cellPad;
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            g.DefaultCellStyle.BackColor = Card;
            g.DefaultCellStyle.ForeColor = GridText;
            // Line, the palette's one lighter neutral, rather than a saturated
            // Cyan/Orange fill: a fully accented row would dominate the screen
            // every time something is selected, which happens constantly here.
            g.DefaultCellStyle.SelectionBackColor = SelectionBg;
            g.DefaultCellStyle.SelectionForeColor = TextPrimary;
            g.DefaultCellStyle.Font = GridCell;
            g.DefaultCellStyle.Padding = cellPad;
            g.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            g.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = SelectionBg;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;

            // Columns only exist once a panel's load*() has set the DataSource,
            // which happens after applyPanel() has run — so size them on the
            // binding event rather than here. Re-entrancy is guarded because
            // changing AutoSizeColumnsMode re-lays-out the grid.
            bool sizing = false;
            g.DataBindingComplete += delegate
            {
                if (sizing) return;
                sizing = true;
                try { sizeColumns(g); }
                finally { sizing = false; }
            };
        }

        /// <summary>
        /// Gives every column the width its own content and header actually need,
        /// then — if they all fit — shares out the leftover space in the same
        /// proportions so the grid fills its frame with no ragged right edge.
        /// A narrow "מזהה" column stays narrow; a long session description gets
        /// the room it needs.
        /// </summary>
        private static void sizeColumns(DataGridView g)
        {
            if (g.Columns.Count == 0 || g.IsDisposed) return;

            float s = scaleOf(g);

            // Measure: widest of (header, every cell) per column.
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            g.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Turn each measured width into a fill weight, so the columns keep
            // their relative proportions — a "מזהה" column stays narrow, a long
            // session description stays wide — while together they consume
            // exactly the grid's width. Fill is used even when the measured
            // widths total more than the grid: compressing proportionally beats
            // a horizontal scrollbar, and MinimumWidth stops any column being
            // squeezed into uselessness.
            foreach (DataGridViewColumn col in g.Columns)
            {
                // AutoResize fits the text exactly; add breathing room so values
                // are not jammed against the cell border.
                int measured = col.Width + dp(s, 10);
                col.MinimumWidth = dp(s, 54);
                col.FillWeight = Math.Min(65535, Math.Max(1, measured));
            }

            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
