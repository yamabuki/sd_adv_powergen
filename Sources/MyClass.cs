﻿/*
 * Created by SharpDevelop.
 * User: sulusdacor
 * Date: 17.11.2016
 * Time: 10:32
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using RimWorld;            // RimWorld specific functions are found here (like 'Building_Battery')
using UnityEngine;         // Always needed
using Verse;               // RimWorld universal objects are here (like 'Building')

namespace sd_adv_powergen
{
    [StaticConstructorOnStartup]
    public class sd_adv_powergen_CompAdvPowerPlantSolar : CompPowerPlant
    {
        private static readonly Vector2 sd_adv_powergen_BarSize = new Vector2(2.3f, 0.14f);

        private static readonly Material sd_adv_powergen_PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));

        private static readonly Material sd_adv_powergen_PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

        private float MinPowerOutput => 0f;
        private float MaxPowerOutput => 3400f;

        protected override float DesiredPowerOutput
        {
            get
            {
                return Mathf.Lerp(this.MinPowerOutput, this.MaxPowerOutput, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
            }
        }

        private float RoofedPowerOutputFactor
        {
            get
            {
                int num = 0;
                int num2 = 0;
                foreach (IntVec3 current in this.parent.OccupiedRect())
                {
                    num++;
                    if (this.parent.Map.roofGrid.Roofed(current))
                    {
                        num2++;
                    }
                }
                return (float)(num - num2) / (float)num;
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
            r.center = this.parent.DrawPos + Vector3.up * 0.1f;
            r.size = sd_adv_powergen_CompAdvPowerPlantSolar.sd_adv_powergen_BarSize;
            r.fillPercent = base.PowerOutput / this.MaxPowerOutput;
            r.filledMat = sd_adv_powergen_CompAdvPowerPlantSolar.sd_adv_powergen_PowerPlantSolarBarFilledMat;
            r.unfilledMat = sd_adv_powergen_CompAdvPowerPlantSolar.sd_adv_powergen_PowerPlantSolarBarUnfilledMat;
            r.margin = 0.15f;
            Rot4 rotation = this.parent.Rotation;
            rotation.Rotate(RotationDirection.Clockwise);
            r.rotation = rotation;
            GenDraw.DrawFillableBar(r);
        }
    }

    [DefOf]
    public static class ThingDefOf
    {
        public static ThingDef sd_adv_powergen_WatermillGenerator;
    }
}