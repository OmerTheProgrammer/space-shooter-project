<%@ Page Title="" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="User Profile.aspx.cs" Inherits="space_shooter.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #Logout_button{
            font-size: 35px;
            width: 150px;
            color:lightcyan;
        }

        #Logout_button:hover{
            color:rgba(242, 255, 255, 0.75);
        }
    </style>
    <script>
        function Logout() {
            //changes user.IsLoggedIn to false, and recreates the user empty.
            //user.IsAdmin = false;
            //user.IsLoggedIn = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-5">
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
                                       <h4>Your Profile</h4>
                                         <span>Account Status - </span> <!-- user/admin -->
                                         <asp:Label class="badge badge-pill badge-info" ID="Label1" runat="server" Text="Your status"></asp:Label>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label class="input text" for="user_id_presenter">User Id: </label>
                                    <div class="form-group">
                                        <asp:TextBox class="form-control input_box" ID="user_id"
                                        runat="server" placeholder="User ID" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label class="input text" for="name_input">Name: </label>
                                    <div class="form-group">
                                        <input type="text" class="input_box" id="name_input" 
                                            placeholder="Write username here: "/>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label class="input text" for="email_input">Email: </label>
                                    <div class="form-group">
                                            <input type="text" class="input_box" id="email_input"
                                                placeholder="Write your email here: "/>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label class="input text" for="birthday_input">Date of Birth: </label>
                                    <div class="form-group">
                                        <input type="date" class="input_box" id="birthday_input"
                                                />
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label class="input text" for="old_password">Old password: </label>
                                    <div class="form-group">
                                        <asp:TextBox class="form-control input_box" ID="old_password"
                                        runat="server" placeholder="Old Password" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label class="input text" for="new_password">New password: </label>
                                    <div class="form-group">
                                        <input type="password" class="input_box" id="new_password"
                                                placeholder="Write password here: "/>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-8 mx-auto">
                                    <center>
                                    <div class="form-group">
                                        <asp:Button class="btn btn-primary btn-block btn-lg" ID="Button1"
                                            runat="server" Text="Update" />
                                        <button type="button" onclick="Logout()" class="button nav_text"
                                            id="Logout_button">Log Out</button>
                                    </div>
                                </center>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col">
                                    <center>
                                        <img width:"100px" src="imgs/books1.png"/>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <center>
                                        <h4>Your Issued Books</h4>
                                        <asp:Label class="badge badge-pill badge-info" ID="Label2" runat="server" Text="your books info"></asp:Label>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:GridView class="table table-striped table-bordered" ID="GridView1" runat="server"></asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
