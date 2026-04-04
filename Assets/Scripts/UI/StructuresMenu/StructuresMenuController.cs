using UnityEngine;

public class StructuresMenuController : MonoBehaviour
{
    private StructuresMenuView view;
    private IEntityController entityController;

    //show all structures
    //when click on them do things or open their respective menu?
    //idk do smtg here
    //like if u have extractor just show production and like info
    //if u have shipyard show build ship stuff
    //if you have teleport then logic for that? dunno what
    //defense building

    public StructuresMenuController(StructuresMenuView view)
    {
        this.view = view;

        ConnectView();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
    }

    public void OpenView(IEntityController entityController)
    {
        this.entityController = entityController;
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }
}
