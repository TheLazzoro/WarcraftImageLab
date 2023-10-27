using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarcraftImageLab.ImageProcessing
{
    internal class WC3IconFactory
    {
        private static bool hasLoaded;
        private static Bitmap Classic_Icon_BTN;
        private static Bitmap Classic_Icon_PAS;
        private static Bitmap Classic_Icon_ATC;
        private static Bitmap Classic_Icon_Infocard;
        private static Bitmap Classic_Icon_Infocard_Upgrade;
        private static Bitmap Classic_Icon_DISBTN;
        private static Bitmap Classic_Icon_DISPAS;
        private static Bitmap Classic_Icon_DISATC;
        private static Bitmap Reforged_Icon_BTN;
        private static Bitmap Reforged_Icon_PAS;
        private static Bitmap Reforged_Icon_ATC;
        private static Bitmap Reforged_Icon_Infocard;
        private static Bitmap Reforged_Icon_Infocard_Upgrade;
        private static Bitmap Reforged_Icon_DISBTN;
        private static Bitmap Reforged_Icon_DISPAS;
        private static Bitmap Reforged_Icon_DISATC;

        BorderModeEnum graphicsMode;

        public WC3IconFactory(BorderModeEnum graphicsMode)
        {
            this.graphicsMode = graphicsMode;

            if (!hasLoaded)
            {
                string dir = Path.Combine(Directory.GetCurrentDirectory(), "Resources/");

                Classic_Icon_BTN = new Bitmap(dir + "Icon_Border.png");
                Classic_Icon_PAS = new Bitmap(dir + "Icon_Border_Passive.png");
                Classic_Icon_ATC = new Bitmap(dir + "Icon_Border_Autocast.png");
                Classic_Icon_Infocard = new Bitmap(dir + "Icon_Border_Attack.png");
                Classic_Icon_Infocard_Upgrade = new Bitmap(dir + "Icon_Border_Attack_Upgrade.png");
                Classic_Icon_DISBTN = new Bitmap(dir + "Icon_Border_Disabled.png");
                Classic_Icon_DISPAS = new Bitmap(dir + "Icon_Border_Disabled.png");
                Classic_Icon_DISATC = new Bitmap(dir + "Icon_Border_Disabled.png");
                Reforged_Icon_BTN = new Bitmap(dir + "Reforged_Icon_Border_Button.png");
                Reforged_Icon_PAS = new Bitmap(dir + "Reforged_Icon_Border_Passive.png");
                Reforged_Icon_ATC = new Bitmap(dir + "Reforged_Icon_Border_Autocast.png");
                Reforged_Icon_Infocard = new Bitmap(dir + "Reforged_Icon_Border_Attack.png");
                Reforged_Icon_Infocard_Upgrade = new Bitmap(dir + "Reforged_Icon_Border_Attack_Upgrade.png");
                Reforged_Icon_DISBTN = new Bitmap(dir + "Reforged_Icon_Border_Disabled.png");
                Reforged_Icon_DISPAS = new Bitmap(dir + "Reforged_Icon_Border_Passive_Disabled.png");
                Reforged_Icon_DISATC = new Bitmap(dir + "Reforged_Icon_Border_Disabled.png");
            }
        }

        internal Bitmap GetBorder(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.BTN:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_BTN;
                    else
                        return Reforged_Icon_BTN;
                
                case IconType.PAS:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_PAS;
                    else
                        return Reforged_Icon_PAS;
                
                case IconType.ATC:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_ATC;
                    else
                        return Reforged_Icon_ATC;
                
                case IconType.DISBTN:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_DISBTN;
                    else
                        return Reforged_Icon_DISBTN;
                    
                case IconType.DISPAS:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_DISPAS;
                    else
                        return Reforged_Icon_DISPAS;
                    
                case IconType.DISATC:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_DISATC;
                    else
                        return Reforged_Icon_DISATC;
                    
                case IconType.ATT:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_Infocard;
                    else
                        return Reforged_Icon_Infocard;
                    
                case IconType.UPG:
                    if (graphicsMode == BorderModeEnum.Classic)
                        return Classic_Icon_Infocard_Upgrade;
                    else
                        return Reforged_Icon_Infocard_Upgrade;
                
                default:
                    return Classic_Icon_BTN;
            }
        }
    }
}
