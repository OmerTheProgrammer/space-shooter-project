<%@ Page Title="Log In" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Log In.aspx.cs" Inherits="space_shooter.pages.Log_In" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #defult_button{
            width:10vw;
        }
        .side.entering_button{
            margin-top: 10vh;
        }
    </style>
    <script>
        function Fill_all() {
            document.getElementById("name_input").value = "user";
            document.getElementById("password_input").value = "p@55word";
        }

        function Submit() {
            user_name = document.getElementById("name_input").value;
            password = document.getElementById("password_input").value;
            var msg = "";

            if (user_name === "" || password === "") {
                msg = "Please fill in all fields.";
            }

            if (print_error(msg)) {
                return;
            }

            //no user_name,email,password checks becouse it's going to be checked in the db, with clean data from the register page.
            alert("user name: " +  user_name);
            alert("password: " + password);

            //if () {
                window.location.href = "Level selection.aspx";
            //}
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container">
            <div class="row">
                <div class="col-md-7 mx-auto">
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
                                        <h1 class="admin-connection title">Log in</h1>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <input onclick="Fill_all()" 
                                        class="btn btn-info btn-block btn-lg side entering_button" 
                                        type="button" value="Defult" />
                                </div>
                                <div class="col-md-4">
                                    <label class="input text" for="name_input">Name: </label>
                                    <div class="form-group">
                                        <input type="text" class="input_box" id="name_input" 
                                            placeholder="Write username here: "/>
                                    </div>
                                    <label class="input text" for="password_input">Password: </label>
                                    <div class="form-group">
                                        <input type="text" class="input_box" id="password_input" 
                                            placeholder="Write password here: "/>
                                    </div>
                                    <center>
                                        <p id="error_text" class="text"></p>
                                    </center>
                                    <div class="form-group">
                                        <input onclick="Submit()" 
                                        class="btn btn-success btn-block btn-lg entering_button" 
                                        id="Submit_button" type="button" value="Submit" />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <input onclick="window.location.href = 'Register.aspx';" 
                                        class="btn btn-info btn-block btn-lg side entering_button" 
                                        type="button" value="To Register" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
