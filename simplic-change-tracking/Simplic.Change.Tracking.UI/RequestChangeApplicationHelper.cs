using Simplic.Framework.DBUI;
using System;


namespace Simplic.Change.Tracking.UI
{
    public class RequestChangeApplicationHelper : GridWindowApplicationHelper<int, RequestChange, RequestChangeViewModel>
    {
        public override string PrimaryKeyColumn => "Ident";

        public override Type WindowInterface => typeof(IRequestChangeWindow);
    }
}
