[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/e4XHBs48)
# Build the Validate SessionState
For this project, you will follow instructions below to create this validate session state project.

1. **Check and Set the Session State.**
    - In the `[Get] Index` we need to check if *Cookie* exists
```csharp
string? json = Request?.Cookies["user"];
if (String.IsNullOrEmpty(json)) 
    return RedirectToAction("UserCheck");

return View(new UserTickets() { User = new User() });
```
-   - the `return` is replaced.
    - Create new action in Home Controller, `[Get] UserCheck`.
        - returns View with new User()
    - Create new action in Home controller, `[Post] UserCheck` which has a User parameter.
        - Check ModelState.IsValid == false, return back to View with the user.
        - Create the Cookie for the unique user...
```csharp
Response.Cookies.Append("user", JsonSerializer.Serialize(user), new CookieOptions() {
    Expires = DateTime.Now.AddMinutes(10)
});
return RedirectToAction("Index");
```
-   -   - the `return` is replaced.
        - Need to add `using System.Text.Json;` for JsonSerializer.
        - The JsonSerializer will convert an object to Json Text which can be stored in Cookie.
        - we set the cookie to expire in 10 min...
    - Check on the View for UserCheck... (UserCheck.cshtml)
        - notice the `<span asp-validation-for="XXXXXX" class="text-danger"></span>`
        - this will show the validation message which we will add next.
    - Back to the Home Controller 
        - we want to add the ClearCookie action so we do not need to wait 10 minutes.
```csharp
[HttpGet]
public IActionResult ClearCookie() 
{
    Response.Cookies.Delete("user");
    return RedirectToAction("Index");
}
```
-   - **Note:** it is a *HttpGet*. This will be called by our button in Index page.
```html
<form asp-action="ClearCookie" method="get">
    <button type="submit" class="btn btn-primary">Clear Cookie</button>
</form>
```
-   - Run to check if this works...
        - put anything into the Email and Phone inputs, press New User.
        - it should go to the Index page and show 3 buttons.
        - Pressing the Clear Cookie button should bring you back to UserCheck page.
![CheckUser Page](/wwwroot/img/Ch11Lab_01.jpg)
![Index Page](/wwwroot/img/Ch11Lab_02.jpg)
   
2. **Add User Validation**
    - Find the User.cs file in Models.
    - Add attribute `[Required]` to the Phone and Email Properties
    - We want to add a Regular Expression validation to the Phone property.
        - `[RegularExpression("\\d{3}\\-\\d{3}\\-\\d{4}", ErrorMessage = "Not correct phone number format, ###-###-####")]`
        - The Phone must be in the: 3 number, dash, 3 number, dash, 4 number format.
    - We want to set the label for Email to "Unique E-Mail Address"
        - `[DisplayName("Unique E-Mail Address")]`
    - And we want to check if the Email property is Unique...
        - `[Remote("CheckEmail", "Home")]`
    - Back to the Home Controller to add the action "CheckEmail" for the remote validation
```csharp
public JsonResult CheckEmail(string email)
{
    if (Data.Users.Any(u => email.Equals(u.Email, StringComparison.InvariantCultureIgnoreCase)))
        return Json($"E-Mail Address {email} is not unique");

    return Json(true);
}
```
-   -   - the `JsonResult` is important, along with the return `Json()`.
        - the return of `true` will PASS validation but anything else will cause FAIL validation.
        - this is called by the form through AJAX, so it will NOT refresh the screen but only effect the input and message.
    - Run and check if its all working...
        - the Email should call the back end.  Copy one of the emails that will cause it to fail (`jose_francis_brown@rocketmail.com`)
        - **Note:** Breakpoints in this code, will not stop the Website.
        - the Phone should only work with the correct phone number format.

3. **Interesting Binding**
    - Back to the Home Controller and the `[Get] Index`
    - Just after the json check, we want to load the User object out of the cookie...
```csharp
User? user = JsonSerializer.Deserialize<User>(json);
var userTickets = new UserTickets() 
{
    User = user,
    Tickets = Data.GetTickets()
};

return View(userTickets);
```
-   -   - the `return` is replaced.
        - The `Data` static opbject is to emulate a database. It is hard coded info, but its good for this demo.
    - Open the Index.cshtml file for some Javascript.
        - We want to add 2 functions, for the buttons below
    - Just below the code block, we add the section Scripts...
```html
@section Scripts {
<script>
    function clearAndHide(i) {
        document.getElementById(`Tickets_${i}_`).value = '';
        document.getElementById(`item${i}`).classList.add('d-none');
    }

    function addNew() {
        const ticketList = document.getElementById('ticketList');
        const i = ticketList.childElementCount;

        const elemInput = document.createElement('input');
        elemInput.id = `Tickets_${i}_`;
        elemInput.name = `Tickets[${i}]`;
        elemInput.className = 'form-control d-inline w-75';

        const elem = document.createElement("li");
        elem.id = `input${i}`;
        elem.className = 'list-group-item'
        elem.appendChild(elemInput)

        ticketList.appendChild(elem);
    }
</script>
}
```
-   - The `clearAndHide(i)` function will be used by the Remove button 
        - it gets elements by id... and sets some properties.
        - by setting the input value to empty string, we are going to remove that ticket from the userTicket list.
    - The `addNew()` function will be used by the Add button
        - it creates a new ticket to add to the ticketList.
        - it counts how many are in the list now 
        - and generates a new `<input>` and `<li>` tag with the Next index.
    - [Javascript Template Literals](https://www.w3schools.com/JS//js_string_templates.asp)
    - to connect the buttons to this code, we need to set the `onclick`
        - To the Add `<a>` tag button, we add `onclick="addNew()"`
        - To the Remove `<a>` tag button, we add `onclick="clearAndHide(@(i))"`
            - the @(i) parameter puts in the Index of the Ticket.
    - Back to the Home Controler to finish up the `[Post] Index`.
        - We need to loop through each Ticket...
            - check if its null or empty and remove it from the list.
```csharp
foreach (string ticket in userTickets.Tickets.ToArray()) 
{
    if (String.IsNullOrEmpty(ticket))
        userTickets.Tickets.Remove(ticket);
}
```
-   -   - **Note:** the ModelState.Clear is important.
        - we will get strange results if it's missing.
    - Run and check if its all working...
        - Start with Add button
        - Should create an input at the bottom of the list, no Remove button. ![Click Add Button](/wwwroot/img/Ch11Lab_03.jpg)
        - Next, Click on one of the Remove Buttons. ![Click Remove Button](/wwwroot/img/Ch11Lab_04.jpg)
        - Then Click on Save button. ![Click Save Button](/wwwroot/img/Ch11Lab_05.jpg)
---
## Bonus...
- You might notice the User.Email is gone once you Save.
    - Fix it so the User.Email is preserved.
- When you click on the Add Button...
    - it would be nice that the `<input>` you just added would be active and ready for typing.
