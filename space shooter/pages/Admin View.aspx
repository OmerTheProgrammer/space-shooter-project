<%@ Page Title="Data" Language="C#" MasterPageFile="~/website_master_page.Master" AutoEventWireup="true" CodeBehind="Admin View.aspx.cs" Inherits="space_shooter.pages.Admin_View" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- library links: -->
    <!-- datatables - CSS: -->
    <link herf="libraries/datatables/css/dataTables.dataTables.min.css" rel="stylesheet"/>

    <!-- datatables - JS: -->
    <script src="../libraries/datatables/js/dataTables.min.js"></script>
    <style>
        .tab-pane.fade{
            margin:1%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section>
        <div class="container">
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <h1 class="title">Admin View</h1>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div class="row">
                <div class="col">
                    <ul class="nav nav-underline" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#home-tab-pane" type="button" role="tab" aria-controls="home-tab-pane" aria-selected="true">Users</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="profile-tab" data-bs-toggle="tab" data-bs-target="#profile-tab-pane" type="button" role="tab" aria-controls="profile-tab-pane" aria-selected="false">Admins</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="contact-tab" data-bs-toggle="tab" data-bs-target="#contact-tab-pane" type="button" role="tab" aria-controls="contact-tab-pane" aria-selected="false">Players</button>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane fade active show" id="home-tab-pane" role="tabpanel" 
                            aria-labelledby="home-tab" tabindex="0">
                            <section>
                                <div class="container">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="card">
                                           <div class="card-body">
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <h4>Users</h4>
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <img width="100px" src="../Assets/Main_Menu/Entering/user.png" />
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col-md-4">
                                                    <label>user's ID:</label>
                                                    <div class="form-group">
                                                          <asp:TextBox CssClass="form-control" ID="TextBox1" runat="server" placeholder="ID"></asp:TextBox>
                                                    </div>
                                                 </div>
                                                 <div class="col-md-8">
                                                    <label>user name:</label>
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <asp:TextBox CssClass="form-control" ID="TextBox2" runat="server" placeholder="Username"></asp:TextBox>
                                                            <asp:Button class="btn btn-primary" ID="Button1" runat="server" Text="Go" />
                                                        </div>
                                                    </div>
                                                 </div>
                                              </div>
                                               <div style="margin:3%"></div>
                                              <div class="row">
                                                 <div class="col-4">
                                                    <asp:Button ID="Button2" class="btn btn-lg btn-block btn-success" runat="server" Text="Add" />
                                                 </div>
                                                 <div class="col-4">
                                                    <asp:Button ID="Button3" class="btn btn-lg btn-block btn-warning" runat="server" Text="Update" />
                                                 </div>
                                                 <div class="col-4">
                                                    <asp:Button ID="Button4" class="btn btn-lg btn-block btn-danger" runat="server" Text="Delete" />
                                                 </div>
                                              </div>
                                           </div>
                                        </div>
                                        <br>
                                     </div>
                                     <div class="col-md-7">
                                        <div class="card">
                                           <div class="card-body">
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <h4>Users: </h4>
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
                        </div>
                        <div class="tab-pane fade" id="profile-tab-pane" role="tabpanel"
                            aria-labelledby="profile-tab" tabindex="0">
                            <section>
                                <div class="container">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="card">
                                           <div class="card-body">
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <h4>Admins</h4>
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <img width="110px" src="../Assets/Main_Menu/Entering/admin.png" />
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col-md-4">
                                                    <label>Admin ID</label>
                                                    <div class="form-group">
                                                       <div class="input-group">
                                                          <asp:TextBox CssClass="form-control" ID="TextBox3" runat="server" placeholder="ID"></asp:TextBox>
                                                          <asp:Button class="btn btn-primary" ID="Button5" runat="server" Text="Go" />
                                                       </div>
                                                    </div>
                                                 </div>
                                                 <div class="col-md-8">
                                                    <label>Admin Name</label>
                                                    <div class="form-group">
                                                       <asp:TextBox CssClass="form-control" ID="TextBox4" runat="server" placeholder="Admin Name"></asp:TextBox>
                                                    </div>
                                                 </div>
                                              </div>
                                               <div style="margin:3%"></div>
                                              <div class="row">
                                                 <div class="col-4">
                                                    <asp:Button ID="Button6" class="btn btn-lg btn-block btn-success" runat="server" Text="Add" />
                                                 </div>
                                                 <div class="col-4">
                                                    <asp:Button ID="Button7" class="btn btn-lg btn-block btn-warning" runat="server" Text="Update" />
                                                 </div>
                                                 <div class="col-4">
                                                    <asp:Button ID="Button8" class="btn btn-lg btn-block btn-danger" runat="server" Text="Delete" />
                                                 </div>
                                              </div>
                                           </div>
                                        </div>
                                        <br>
                                     </div>
                                     <div class="col-md-7">
                                        <div class="card">
                                           <div class="card-body">
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <h4>Admin List</h4>
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col">
                                                    <asp:GridView class="table table-striped table-bordered" ID="GridView2" runat="server"></asp:GridView>
                                                 </div>
                                              </div>
                                           </div>
                                        </div>
                                     </div>
                                  </div>
                               </div>
                            </section>
                        </div>
                        <div class="tab-pane fade" id="contact-tab-pane" role="tabpanel" 
                            aria-labelledby="contact-tab" tabindex="0">
                            <section>
                                <div class="container">
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="card">
                                           <div class="card-body">
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <h4>Players</h4>
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <img width="110px" src="../Assets/player.png" />
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col-md-4">
                                                    <label>Player ID</label>
                                                    <div class="form-group">
                                                       <div class="input-group">
                                                          <asp:TextBox CssClass="form-control" ID="TextBox5" runat="server" placeholder="ID"></asp:TextBox>
                                                          <asp:Button class="btn btn-primary" ID="Button9" runat="server" Text="Go" />
                                                       </div>
                                                    </div>
                                                 </div>
                                                 <div class="col-md-8">
                                                    <label>Player Name</label>
                                                    <div class="form-group">
                                                       <asp:TextBox CssClass="form-control" ID="TextBox6" runat="server" placeholder="Admin Name"></asp:TextBox>
                                                    </div>
                                                 </div>
                                              </div>
                                               <div style="margin:3%"></div>
                                              <div class="row">
                                                 <div class="col-4">
                                                    <asp:Button ID="Button10" class="btn btn-lg btn-block btn-success" runat="server" Text="Add" />
                                                 </div>
                                                 <div class="col-4">
                                                    <asp:Button ID="Button11" class="btn btn-lg btn-block btn-warning" runat="server" Text="Update" />
                                                 </div>
                                                 <div class="col-4">
                                                    <asp:Button ID="Button12" class="btn btn-lg btn-block btn-danger" runat="server" Text="Delete" />
                                                 </div>
                                              </div>
                                           </div>
                                        </div>
                                        <br>
                                     </div>
                                     <div class="col-md-7">
                                        <div class="card">
                                           <div class="card-body">
                                              <div class="row">
                                                 <div class="col">
                                                    <center>
                                                       <h4>Player List</h4>
                                                    </center>
                                                 </div>
                                              </div>
                                              <div class="row">
                                                 <div class="col">
                                                    <asp:GridView class="table table-striped table-bordered" ID="GridView3" runat="server"></asp:GridView>
                                                 </div>
                                              </div>
                                           </div>
                                        </div>
                                     </div>
                                  </div>
                               </div>
                            </section>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
