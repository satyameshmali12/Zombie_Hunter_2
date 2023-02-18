using Godot;
using Godot.Collections;
using System.Collections;

public class Sell_Zombies : Basic_View
{

    Data_Manager zombieDataManager, userData;
    ArrayList availableZombies, sellingZombies, availableZombiesCopy = new ArrayList();

    ArrayList zombieNames;
    int avaZomNodeRenderIdx = 0;
    int sellingZomNodeRenderIdx = 0;




    Button pressedButton = null;
    public override void _Ready()
    {
        base._Ready();

        zombieDataManager = new Data_Manager(basf.global_paths.Zombie_Data_Field_Url);
        userData = new Data_Manager(basf.global_paths.User_Data_Field_Url);
        userData.load_data("Aj");

        zombieNames = zombieDataManager.get_set_of_field_values("Zombie_Name");

        availableZombies = new ArrayList();
        sellingZombies = new ArrayList();


        foreach (string zombieName in zombieNames)
        {
            zombieDataManager.load_data(zombieName);
            availableZombies.Add(
                new Dictionary<string, string>()
                {
                    { "Name", zombieName },
                    { "Available_Count", zombieDataManager.get_data("Num_Of_Catched_Zombies") },
                    { "Add_Count_Label_Text", "1" }
                }
            );
            sellingZombies.Add(
                new Dictionary<string, string>()
                    {
                        { "Name", zombieName },
                        { "Available_Count", "0" },
                        { "Add_Count_Label_Text", "1" },
                        { "Per_Zombie_Price", zombieDataManager.get_data("Per_Zombie_Price") }
                    }
            );
        }

        availableZombiesCopy = availableZombies.Clone() as ArrayList;

    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        avaZomNodeRenderIdx = renderData(this.GetNode<Control>("Available_Zombie_Data"), availableZombies, avaZomNodeRenderIdx);
        sellingZomNodeRenderIdx = renderData(this.GetNode<Control>("Selling_Zombie_Data"), sellingZombies, sellingZomNodeRenderIdx);


        this.GetNode<Label>("Available_Money").Text = userData.get_data("Money");

        if (pressedButton != null && !pressedButton.Pressed)
        {
            pressedButton = null;
        }

        if (pressedButton == null)
        {
            Control sellNode = this.GetNode<Control>("Sell");
            Button sellButton = sellNode.GetNode<Button>("Sell_Button");
            Label sellingPriceLabel = sellNode.GetNode<Label>("Selling_Price");

            int sellingPrice = 0;

            foreach (Dictionary<string, string> item in sellingZombies)
            {
                sellingPrice += (int.Parse(item["Available_Count"]) * int.Parse(item["Per_Zombie_Price"]));
            }

            sellingPriceLabel.Text = sellingPrice.ToString();

            if (sellButton.Pressed)
            {
                pressedButton = sellButton;

                availableZombiesCopy = availableZombies.Clone() as ArrayList;

                int updatedMoney = int.Parse(userData.get_data("Money")) + sellingPrice;
                userData.set_value("Money", updatedMoney.ToString());
                userData.save_data();
                userData.load_previous_data_again();

                // removing all the sellingZombies
                foreach (Dictionary<string, string> data in sellingZombies)
                {
                    data["Available_Count"] = "0";
                }

                // saving the availableZombies back to the zombie data
                foreach (Dictionary<string, string> data in availableZombies)
                {
                    zombieDataManager.load_data(data["Name"]);
                    zombieDataManager.set_value("Num_Of_Catched_Zombies", data["Available_Count"]);
                }
                zombieDataManager.save_data();


            }

        }

    }


    public int renderData(Control mainNode, ArrayList zombieData, int renderIdx)
    {

        // bool isDataLoadedConfigured = false;
        HBoxContainer boxContainer = mainNode.GetNode<HBoxContainer>("HBoxContainer");
        var dataBoxes = boxContainer.GetChildren();

        Button leftButton = mainNode.GetNode<Button>("Left");
        Button rightButton = mainNode.GetNode<Button>("Right");

        Button removeAllButton = mainNode.GetNode<Button>("Remove_All");

        int totalZombie = 0;
        foreach (Dictionary<string, string> data in zombieData)
        {
            totalZombie += int.Parse(data["Available_Count"]);
        }

        boxContainer.Visible = (totalZombie != 0);
        mainNode.GetNode<Label>("No_Zombie_Label").Visible = !boxContainer.Visible;

        removeAllButton.Visible = (totalZombie != 0);


        // filteringData
        ArrayList filterData = new ArrayList();
        foreach (Dictionary<string, string> data in zombieData)
        {
            if (int.Parse(data["Available_Count"]) > 0)
            {
                filterData.Add(data);
            }
        }


        // disvisibling the left and the right button if not usefull
        leftButton.Visible = renderIdx != 0 && filterData.Count != 0 || !boxContainer.Visible;
        rightButton.Visible = renderIdx + dataBoxes.Count < filterData.Count && filterData.Count != 0 || !boxContainer.Visible;


        foreach (VBoxContainer dataBox in dataBoxes)
        {

            var boxIdx = dataBoxes.IndexOf(dataBox);
            var itemIdx = boxIdx + renderIdx;

            if (itemIdx < filterData.Count)
            {

                Dictionary<string, string> itemData = filterData[itemIdx] as Dictionary<string, string>;

                dataBox.Visible = true;


                Label nameLabel = dataBox.GetNode<Label>("MarginContainer/VBoxContainer/BG/Name");
                Label avaCountLabel = dataBox.GetNode<Label>("MarginContainer/VBoxContainer/BG/Available_Count");

                string zombieName = itemData["Name"];

                nameLabel.Text = zombieName;

                int available_count = int.Parse(itemData["Available_Count"]);

                avaCountLabel.Text = $"Count:-{available_count}";


                Button countDecreButton = dataBox.GetNode<Button>("Adding_Button/Decrease");
                Button countIncreButton = dataBox.GetNode<Button>("Adding_Button/Increase");
                Label addingCountLabel = dataBox.GetNode<Label>("Adding_Button/Adding_Count");
                Button dataManipulateButton = dataBox.GetNode<Button>("MarginContainer/VBoxContainer/BG/Manipulate_Data_Button");



                void setItemAddCountLabText(int newCount)
                {
                    newCount = (newCount >= 1 && newCount <= available_count) ? newCount : (newCount<1)?available_count:1;
                    addingCountLabel.Text = newCount.ToString();
                    itemData["Add_Count_Label_Text"] = newCount.ToString();
                }


                int newAddCount = int.Parse(itemData["Add_Count_Label_Text"]);
                setItemAddCountLabText(newAddCount);

                int addCountIncre = (countDecreButton.Pressed) ? -1 : (countIncreButton.Pressed) ? 1 : 0;

                if (addCountIncre != 0)
                {
                    int newCount = int.Parse(addingCountLabel.Text) + addCountIncre;
                    setItemAddCountLabText(newCount);
                }



                if (pressedButton == null)
                {
                    if (dataManipulateButton.Pressed)

                    {
                        int addCount = int.Parse(addingCountLabel.Text);
                        if (zombieData == availableZombies)
                        {
                            shiftZomFromOneToAno(zombieName, addCount, sellingZombies, availableZombies);
                        }
                        else
                        {
                            shiftZomFromOneToAno(zombieName, addCount, availableZombies, sellingZombies);
                        }

                        available_count = int.Parse(itemData["Available_Count"]); // getting the updated available count as that of the filter data

                        if (available_count < int.Parse(addingCountLabel.Text))
                        {
                            setItemAddCountLabText(available_count);
                        }

                        pressedButton = dataManipulateButton;
                    }

                    int change = (rightButton.Pressed) ? 1 : (leftButton.Pressed) ? -1 : 0;
                    if (change != 0)
                    {
                        int newIndex = renderIdx + change;
                        if (newIndex >= 0 && newIndex + dataBoxes.Count < filterData.Count)
                        {
                            renderIdx = newIndex;
                            break;
                        }
                    }
                }


            }
            else
            {
                dataBox.Visible = false;
            }
        }


        if (removeAllButton.Pressed)
        {
            foreach (Dictionary<string, string> data in zombieData)
            {
                string zombieName = data["Name"];
                int zombieCount = int.Parse(data["Available_Count"]);
                if (zombieData == availableZombies)
                {
                    shiftZomFromOneToAno(zombieName, zombieCount, sellingZombies, availableZombies);
                }
                else
                {
                    shiftZomFromOneToAno(zombieName, zombieCount, availableZombies, sellingZombies);
                }
            }
        }

        // if (!isDataLoadedConfigured)
        // {
        //     isDataToggled = false;
        // }
        // if (isDataToggled)
        // {
        //     GD.Print("Hey I am there");

        // }


        return renderIdx;
    }

    public void shiftZomFromOneToAno
    (string zombieName, int addCount, ArrayList addingList, ArrayList removingList)
    {

        addingList = updateArraylistDictionary("Name", zombieName, "Available_Count", (addCount), addingList);
        removingList = updateArraylistDictionary("Name", zombieName, "Available_Count", (-addCount), removingList);

    }


    public ArrayList updateArraylistDictionary(string identifer, string identiferValue, string settlingIdentifier, int change, ArrayList list)
    {
        foreach (Godot.Collections.Dictionary<string, string> data in list)
        {
            string currentIdentifierValue = data[identifer];
            if (currentIdentifierValue == identiferValue)
            {
                data[settlingIdentifier] = (int.Parse(data[settlingIdentifier]) + change).ToString();
            }
        }
        return list;

    }
}
