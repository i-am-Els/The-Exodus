
namespace Behaviors
{
    public interface IUpgradeable
    {
        public void UpgradeItem(string item);

        public void DowngradeItem(string item);
    }
}