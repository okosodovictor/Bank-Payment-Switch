using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.SinkNodeManagement
{
  public  class AddSinkNode:EntityUI<SinkNode>
    {
      string message = "";
      public AddSinkNode()
      {
          // SinkNodeManager _sinkNode = new SinkNodeManager();
            AddSection()
                 .WithTitle("Add New Sink Node")
                 .WithFields(
                 new List<IField>()
                {
                    Map(x => x.Name).AsSectionField<TextBox>().TextFormatIs(TextFormat.name).WithLength(100).Required(),
                    Map(x => x.HostName).AsSectionField<TextBox>().TextFormatIs(@"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9-]*[a-zA-Z0-9]).)*([A-Za-z]|[A-Za-z][A-Za-z0-9-]*[A-Za-z0-9])$").WithLength(50).Required(),
                    Map(x => x.IPAddress).AsSectionField<TextBox>().WithLength(20).TextFormatIs(@"^(0[0-7]{10,11}|0(x|X)[0-9a-fA-F]{8}|(\b4\d{8}[0-5]\b|\b[1-3]?\d{8}\d?\b)|((2[0-5][0-5]|1\d{2}|[1-9]\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))(\.((2[0-5][0-5]|1\d{2}|\d\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))){3})$").Required(),
                    Map(x => x.Port).AsSectionField<TextBox>().TextFormatIs(TextFormat.numeric).WithLength(10).Required(),
                });

                AddButton()
                  .WithText("Add Sink Node")
                  .SubmitTo(x => 
                  {
                      var result=false;
                      try
                      {
                          //x.IsActive = false;
                         result  = new SinkNodeManager().AddSinkNode(x);
                      }
                      catch (Exception ex) 
                      {
                          while (ex.InnerException != null) ex = ex.InnerException;
                          message = ex.Message;
                          throw; 
                      }
                    return result;
                  }).OnSuccessDisplay("Saved Successfully")
                  .OnFailureDisplay(string.Format("Failed to Save SinkNode:{0}", message));       
        }
    }
}
