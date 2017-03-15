﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Game.Modes;

namespace osu.Game.Screens.Select.Leaderboards
{
    public class Leaderboard : Container
    {
        private ScrollContainer scrollContainer;
        private FillFlowContainer<LeaderboardScore> scrollFlow;

        private IEnumerable<Score> scores;
        public IEnumerable<Score> Scores
        {
            get { return scores; }
            set
            {
                scores = value;

                int i = 0;
                if (scores == null)
                {
                    foreach (var c in scrollFlow.Children)
                        c.FadeOut(150 + i++ * 10);
                    return;
                }

                scrollFlow.Clear();

                i = 0;
                foreach(var s in scores)
                {
                    var ls = new LeaderboardScore(s, 1 + i++)
                    {
                        AlwaysPresent = true,
                        State = Visibility.Hidden,
                    };
                    scrollFlow.Add(ls);

                    ls.Delay((i - 1) * 50, true);
                    ls.Show();
                }

                scrollContainer.ScrollTo(0f, false);
            }
        }

        protected override void Update()
        {
            base.Update();

            foreach (var s in scrollFlow.Children)
            {
                var topY = scrollContainer.ScrollContent.DrawPosition.Y + s.DrawPosition.Y;
                var bottomY = topY + 70;
                var fadeStart = scrollContainer.DrawHeight - 10;
                fadeStart += scrollContainer.IsScrolledToEnd() ? 70 : 0;

                s.ColourInfo = ColourInfo.GradientVertical(Color4.White.Opacity(System.Math.Min((fadeStart - topY) / 70, 1)),
                                                           Color4.White.Opacity(System.Math.Min((fadeStart - bottomY) / 70, 1)));
            }
        }

        public Leaderboard()
        {
            Children = new Drawable[]
            {
                scrollContainer = new ScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ScrollDraggerVisible = false,
                    Children = new Drawable[]
                    {
                        scrollFlow = new FillFlowContainer<LeaderboardScore>
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(0f, 5f),
                            Padding = new MarginPadding(5),
                        },
                    },
                },
            };
        }
    }
}
