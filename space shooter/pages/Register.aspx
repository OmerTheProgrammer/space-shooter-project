<%@ Page Title="Register" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="space_shooter.pages.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #first_object{
            margin-left:40%;
        }
        
        #agrees{
            width:20px;
            height:20px;
        }

        #terms {
            position: fixed;  /* Ensures the popup stays in place when scrolling */
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);  /* Positions the popup in the center */
            padding: 20px;
            border: 1px solid #ccc;
            display: none;  /* Hide the popup initially */
            height:72vh;
            width:60vw;
        }

        #to_terms{
            font-size:35px;
        }
    </style>
    <script>
        function Submit() {
            user_name = document.getElementById("name_input").value;
            email = document.getElementById("email_input").value;
            password = document.getElementById("password_input").value;
            password_again = document.getElementById("password_again").value;

            let msg = "";
            if (user_name === "" || email === "" || password === "" || password_again === "") {
                msg = "Please fill in all fields.";
            }

            if (print_error(msg)) {
                return;
            }

            /*email check
                format: /=start&end, (...)=part, ^=start, $=end, [...]=allowed characters in part, \.=.,   *= 0+ characters, += 1+ characters, ...|... =both options are ok, \w=0-9,a-z, A-Z, \s=space, tab, newline, \d=0-9 so:
                part one must have 1 at least: <,>,(,),[,],\,., , ,;,:, then we have @ then part 2 must have 1 at least: 0-9,a-z, A-Z.
            */
            const email_fromat = /^[\w-.]+@([\w-.]+\.)+[a-zA-Z]{2,}$/, attIndex = email.indexOf('@'), e_length = email.length;
            //smallest email: oo@oo.oo , which is 8 characters.
            if (e_length < 8) {//len-start
                msg = "Email too short, shorter then 8 characters";
            }
            else if (e_length > 20) {//len-end
                msg = "Email too long, longer then 20 characters";
            }
            else if (attIndex < 2) {//att-start
                msg = "The @(Atmark) sign must be 2 digits away from start.";
            }
            else if (attIndex === e_length - 2) {//att-end
                msg = "The @(Atmark) sign must be 2 digits away from end. ";
            }
            else if (email.lastIndexOf('.') === -1) {//dot-start
                msg = "A .(Dot) sign must be in the email address. ";
            }
            else if ((email.indexOf('.', attIndex + 2) === (attIndex + 2))) {
                msg = "A .(Dot) sign must be at least 2 digits away from the @(Atmark) sign. ";
            }
            else if (email.indexOf('.', attIndex + 2) === -1) {
                msg = "A .(Dot) sign must be after the @(Atmark) sign. ";
            }
            else if (email.lastIndexOf('.') === e_length - 2) {
                msg = "A .(Dot) sign must be at least 2 digits away from the end. ";
            }
            else if (email.indexOf('.') < 2) {//dot-end
                msg = "A .(Dot) sign must be at least 2 digits away from the start. ";
            }
            else if (!email_fromat.test(email)) {//format
                msg = "Email needs to be in this format: example@example.com";
            }

            if (print_error(msg)) {
                return;
            }

            //password check
            msg = "";
            const minPasswordLength = 8;
            if (password_again !== password) {
                msg = "The password again must be the same!";
            }
            else if (password.length < minPasswordLength) {
                msg = `Password must be at least ${minPasswordLength} characters long.\n But its ${password.length} characters long.`;
            }
            else if (password.length > 20) {
                msg = `Password must be at less then 20 characters long.\n But its ${password.length} characters long.`;
            }
            else if (!(/[^\w\s]/.test(password))) {//has symbols
                msg = 'Password must contain at least one symbol.';
            }
            else if (!(/[\d]/.test(password))) {//numbers
                msg = 'Password must contain at least one number.';
            }

            if (print_error(msg)) {
                return;
            }

            //<input type="date"> returns only dd/mm/yyyy
            date = new Date(document.getElementById("birthday_input").value);
            const now = Date.now();//now in ms
            const msInYear = 1000 * 60 * 60 * 24 * 365.25; // Accounting for leap years, ms*s*min*h*day(in year)
            const msIn120Years = msInYear * 120;//120y in ms

            msg = "";
            if (date < new Date(now - msIn120Years)) {
                msg = "too old, you are dead.";
            }
            else if (date > new Date(now)) {
                msg = "too young, you were never born.";
            }

            if (print_error(msg)) {
                return;
            }

            if (!document.getElementById("agrees").checked) {
                msg = 'You must check the agree box.';
            }

            if (print_error(msg)) {
                return;
            }

            /*var xhr = new XMLHttpRequest();
            xhr.open("POST", "https://localhost:44356/pages/Register.aspx/Submit", true);
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            var data = "user_name=" + encodeURIComponent(user_name) +
                "&email=" + encodeURIComponent(email) +
                "&password=" + encodeURIComponent(password) +
                "&date=" + encodeURIComponent(date);
            xhr.send(data);

            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        // Handle successful response
                        console.log("Data successfully submitted to C# function: ");
                        console.log(xhr);
                        
                    } else {
                        // Handle broken response
                        console.error("Error submitting data to C# function:", xhr.status, xhr.statusText);
                        alert("An error occurred while submitting the data. Please try again later.");
                    }
                }
            };*/
            <%
                //להצגה אפשר ללחוץ רק פעם אחת ואז מוחקים את המידע מהטבלה
                //אם השורה:
                //CONSTRAINT [FK_Users_Conditions] FOREIGN KEY ([User ID]) REFERENCES [Conditions]([User ID])
                //Usersלא נמצאת ב
                //Submit("5","7","4","2024-08-23");
            %>

            if (print_error(msg)) {
                return;
            }
            else {
                window.location.href = "Level selection.aspx";
            }
        }

        function close_terms() {
            terms.style.display = 'None';
            document.getElementById("agrees").checked = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
       <div class="container-fluid">
          <div class="row">
             <div class="col-md-5 mx-auto">
                <div class="card">
                   <div class="card-body">
                      <div class="row">
                         <div class="col">
                            <center>
                                <img src="../Assets/Main_Menu/Entering/user.png"
                                    style="height:32vh"/>
                            </center>
                         </div>
                      </div>
                      <div class="row">
                         <div class="col">
                            <center>
                               <h1 class="title">Register</h1>
                            </center>
                         </div>
                      </div>

                      <div class="row">
                         <div class="col-md-6">
                            <label class="input text" for="name_input">Name: </label>
                            <div class="form-group">
                                <input type="text" class="input_box" id="name_input" 
                                    placeholder="Write username here: "/>
                            </div>
                         </div>
                        <div class="col-md-6">
                            <label class="input text" for="email_input">Email: </label>
                            <div class="form-group">
                                    <input type="text" class="input_box" id="email_input"
                                        placeholder="Write the email here: "/>
                            </div>
                         </div>
                      </div>
                      <div class="row">
                        <div class="col-md-6">
                            <label class="input text" for="password_input">Password: </label>
                            <div class="form-group">
                                    <input type="password" class="input_box" id="password_input"
                                        placeholder="Write password here: "/>
                            </div>
                         </div>
                         <div class="col-md-6">
                            <label class="input text" for="birthday_input">Date of Birth: </label>
                            <div class="form-group">
                                <input type="date" class="input_box" id="birthday_input"/>
                            </div>
                         </div>
                      </div>
                      <div class="row">
                         <div class="col-md-6">
                            <label class="input text" for="password_again">Repeat the password: </label>
                            <div class="form-group">
                                <input type="password" class="input_box" id="password_again"
                                    placeholder="Write same password: "/>
                            </div>
                         </div>
                         <div class="col-md-6">
                            <label class="input text" for="agrees">
                                Do you agree to these
                                <a class="link" onclick="terms.style.display = 'block';" id="to_terms">
                                     terms
                                </a>
                                :
                            </label>
                            <input type="checkbox" class="input_box" id="agrees"/><br />
                         </div>
                      </div>
                        <div class="row">
                         <div class="col">
                            <center>
                                <p id="error_text" class="text" style="margin-left:0;"></p>
                            </center>
                         </div>
                      </div>
                      <div class="row">
                         <div class="col-6">
                            <div class="form-group">
                                        <input onclick="Submit()" 
                                        class="btn btn-success btn-block btn-lg entering_button" 
                                        id="Submit_button" type="button" value="Submit" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="form-group">
                                    <input onclick="window.location.href = 'Log In.aspx';" 
                                        class="btn btn-info btn-block btn-lg entering_button" 
                                        type="button" value="To Log in" />
                            </div>
                        </div>
                      </div>
                   </div>
                </div>
             </div>
          </div>
       </div>
    </section>
    <section>
        <div class="popup" id="terms">
            <div class="m_container">
                <div class="m_card">
                    <h1 class="popup title">Terms of Service:</h1>
                </div>
                <div class="m_card">
                    <p class="popup text">
                        By playing Space Shooters, you agree to our terms:<br />
                            1.Secure your account (you're responsible).<br/>
                            2.Game content is copyrighted (don't distribute).<br/>
                            3.Game is provided "as is" (no warranties), use at your own risk.<br/>
                            4.Terms may be updated (check website).<br/>
                            5.We reserve the right to terminate access.<br/>
                            6.Governed by Israeli law.<br />
                            7.Disputes: omer17508@gmail.com.<br/>
                            8.These terms are the full agreement.<br/>
                        Enjoy Space Shooters!<br/>
                    </p>
                </div>
                <div class="m_card">
                    <button type="button" onclick="close_terms()" class="button" 
                        style="margin-left:45%;">Close</button>
                </div>
            </div>
        </div>
    </section>
    <div style="margin-top:10vh;"></div>
</asp:Content>
