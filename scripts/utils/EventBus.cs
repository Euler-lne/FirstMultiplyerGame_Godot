using System;
using Godot;

namespace Euler.EventBus
{


    // public class HealthEvent
    // {
    //     public static event Action<CharacterBody2D> DieEvent;
    //     public static void CallDieEvent(CharacterBody2D character)
    //     {
    //         DieEvent?.Invoke(character);
    //     }
    // }
    // 不能用全局静态事件，用本地的普通事件可以，否则会出现问题
}