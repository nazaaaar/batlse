using RMC.Mini.View;
using System;

public interface IRewardedAdsButton: IView
{
    event Action OnAdClicked;
}