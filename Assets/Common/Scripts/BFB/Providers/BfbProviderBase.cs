using Biofeedback;

namespace Common.BFB
{
    public abstract class BfbProviderBase
    {
        protected IDataProviderBFB bfb { get; private set;}


        BfbProviderBase() { }

        public BfbProviderBase(IDataProviderBFB bfb)
        {
            this.bfb = bfb;
        }

        public abstract void Deinit();
    }
}