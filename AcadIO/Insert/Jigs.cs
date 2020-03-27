using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace AcadIO.Insert {
    /// <summary>
    /// This code is from the AutoCAD ObjectARX Tutorial and It Just Works (tm).
    /// Honestly, I have never touched it.
    /// </summary>
    public class Jigs {
        public class InsertBlockJig : EntityJig {
            protected Point3d Position;
            protected BlockReference BlockRef;
            public InsertBlockJig(BlockReference BlockRef) : base(BlockRef) {
                this.BlockRef = BlockRef;
                this.Position = BlockRef.Position;
            }
            protected override SamplerStatus Sampler(JigPrompts prompts) {
                string msg = Environment.NewLine + "Specify the insertion point: ";
                JigPromptPointOptions jppo = new JigPromptPointOptions(msg) {
                    UserInputControls = (UserInputControls.Accept3dCoordinates | UserInputControls.NullResponseAccepted)
                };
                PromptPointResult ppr = prompts.AcquirePoint(jppo);
                if (this.Position.DistanceTo(ppr.Value) < Tolerance.Global.EqualPoint) {
                    return SamplerStatus.NoChange;
                }
                else {
                    this.Position = ppr.Value;
                }
                return SamplerStatus.OK;
            }
            protected override bool Update() {
                this.BlockRef.Position = this.Position;
                return true;
            }
        }

        public class RotateBlockJig : EntityJig {
            protected BlockReference BlockRef;
            protected double Rot;
            protected double UcsRot;
            public RotateBlockJig(BlockReference BlockRef) : base(BlockRef) {
                this.BlockRef = BlockRef;
                this.UcsRot = BlockRef.Rotation;
            }
            protected override SamplerStatus Sampler(JigPrompts prompts) {
                JigPromptAngleOptions jpao = new JigPromptAngleOptions(Environment.NewLine + "Specify the rotation: ") {
                    DefaultValue = 0.0,
                    UseBasePoint = true,
                    BasePoint = this.BlockRef.Position,
                    Cursor = CursorType.RubberBand,
                    UserInputControls = (UserInputControls.Accept3dCoordinates | UserInputControls.UseBasePointElevation | UserInputControls.NullResponseAccepted)
                };
                PromptDoubleResult pdr = prompts.AcquireAngle(jpao);
                if (this.Rot == pdr.Value) {
                    return SamplerStatus.NoChange;
                }
                else {
                    this.Rot = pdr.Value;
                }
                return SamplerStatus.OK;
            }
            protected override bool Update() {
                this.BlockRef.Rotation = this.Rot + this.UcsRot;
                return true;
            }
        }

        public class AttributeOverrule {
            private static KeepStraightOverrule myOverrule;
            public static void ImplementOverrule() {
                if (myOverrule == null) {
                    myOverrule = new KeepStraightOverrule();
                    Overrule.AddOverrule(RXClass.GetClass(typeof(AttributeReference)), myOverrule, false);
                }
                Overrule.Overruling = true;
            }
        }
        public class KeepStraightOverrule : TransformOverrule {
            public override void TransformBy(Entity entity, Matrix3d transform) {
                base.TransformBy(entity, transform);
                AttributeReference attRef = (AttributeReference)entity;
                attRef.Rotation = 0.0;
            }
        }

    }
}