using System;

public class ConfirmPopupParam : OpenParam
{
    public string Title;
    public string TargetId;
    public Action OnConfirm;
    public Action OnCancel;
}
