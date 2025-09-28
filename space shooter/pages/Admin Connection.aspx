<%@ Page Title="Admin Connection" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Admin Connection.aspx.cs" Inherits="space_shooter.pages.Admin_Connection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        input{
            width:200px;
        }
    </style>
    <script>
        function Submit() {
            //change to a hidden key with the server
            key = document.getElementById("key_box").value;
            msg = "";
            if (key === "") {
                msg = "Please fill in the field.";
            }

            if (print_error(msg)) {
                return;
            }

            if (key === "superbaruch@17") {
                window.location.href = 'Admin View.aspx';
                //the register isn't working so i don't know if this works.
                <% //SQL_Helper.DoProcedure("Space_Shooter_DB.mdf", "AddAdmin",null);%>
            }
            else {
                msg = "Wrong Key!!!";
            }

            if (print_error(msg)) {
                return;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container">
            <div class="row">
                <div class="col-md-6 mx-auto">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col">
                                    <center>
                                        <img src="../Assets/Main_Menu/Entering/admin.png"
                                            style="height:35vh"/>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <center>
                                        <h1 class="admin-connection title">Admin Connection</h1>
                                    </center>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-7">
                                    <label class="input text">Key: </label>
                                    <div class="form-group">
                                        <input type="password" class="input_box" id="key_box" placeholder="Key: "/>
                                        <center>
                                            <p id="error_text" class="text"></p>
                                        </center>
                                    </div>
                                    <div class="form-group">
                                        <input onclick="Submit()" class="btn btn-success btn-block btn-lg entering_button" id="Submit_button" type="button" value="Submit" />
                                        <input onclick="window.location.href = 'Register.aspx';" class="btn btn-info btn-block btn-lg entering_button" type="button" value="Register" />
                                    </div>
                                </div>
                                <div class="col-md-2"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
