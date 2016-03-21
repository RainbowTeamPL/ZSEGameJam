#pragma strict

private var guiShow : boolean = false;

var wilk : GameObject;

var rayLength = 10;

function Update()
{
    var hit : RaycastHit;
    var fwd = transform.TransformDirection(Vector3.forward);

    if(Physics.Raycast(transform.position, fwd, hit, rayLength))
    {
        if(hit.collider.gameObject.tag == "wilk")
        {
            guiShow = true;
            if(Input.GetKeyDown(KeyCode.R))
            {
                //door.animation.Play("DoorOpen");
                //isOpen = true;
                guiShow = false;
                Destroy(wilk);
            }
        }
    }

    else
    {
        guiShow = false;
    }
}

function OnGUI()
{
    if(guiShow == true)
    {
        GUI.Box(Rect(Screen.width / 2, Screen.height / 2, 100, 25), "Use Door");
    }
}