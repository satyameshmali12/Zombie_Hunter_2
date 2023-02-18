using Godot;
using System;

public class Counter : Control
{

    bool isDataSettled = true;
    int maximumCount= 100;
    int minimumCount = 0;

    Button leftButton,rightButton;
    int currentIndex = 1;

    public override void _Ready()
    {
        leftButton = this.GetNode<Button>("Decrease");
        rightButton = this.GetNode<Button>("Increase");
    }

    public override void _Process(float delta)
    {

        int change = (leftButton.Pressed)?-1:(rightButton.Pressed)?1:0;
        if(change!=0 || isDataSettled)
        {
            int newValue = currentIndex+change;
            currentIndex = (newValue>=1 && newValue<=maximumCount)?newValue:(newValue<1)?maximumCount:minimumCount;
            isDataSettled = false;
        }

        this.GetNode<Label>("Adding_Count").Text = currentIndex.ToString();
    }

    public void setMinMax(int maximumCount,int minimumCount = 0)
    {
        this.maximumCount = maximumCount;
        this.minimumCount = minimumCount;
        isDataSettled = true;
    }

    public int getCurrentCount()
    {
        return currentIndex;
    }
}
