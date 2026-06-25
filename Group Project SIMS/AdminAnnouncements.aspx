<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminAnnouncements.aspx.cs" Inherits="SoftwareEngineeringProject.AdminAnnouncements" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Announcements</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        
        <div class="bg-dark text-white p-3 d-flex justify-content-between align-items-center mb-4 shadow-sm">
            <h4 class="m-0 fw-bold text-warning">Admin Control Panel</h4>
        </div>

        <div class="container">
            <h2 class="mb-4">Manage Campus Announcements</h2>

            <div class="card shadow-sm mb-5" style="max-width: 600px;">
                <div class="card-header bg-primary text-white fw-bold">
                    Create New Announcement
                </div>
                <div class="card-body">
                    
                    <div class="mb-3">
                        <label class="form-label fw-bold">Title</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="e.g., Campus Closed for Holiday"></asp:TextBox>
                    </div>
                    
                    <div class="mb-3">
                        <label class="form-label fw-bold">Content</label>
                        <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Type your message here..."></asp:TextBox>
                    </div>
                    
                    <asp:Button ID="btnPost" runat="server" Text="Post Announcement" CssClass="btn btn-primary fw-bold px-4" OnClick="btnPost_Click" />
                    
                    <div class="mt-3">
                        <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold d-block"></asp:Label>
                    </div>
                    
                </div>
            </div>

            <div class="card shadow-sm mb-5">
                <div class="card-header bg-secondary text-white fw-bold">
                    Announcement History Log
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvAnnouncements" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered table-striped mb-0">
                        <Columns>
                            <asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="Content" HeaderText="Message" />
                            <asp:BoundField DataField="AuthorName" HeaderText="Posted By" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="p-3 text-muted fst-italic">No announcements have been posted yet.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            
        </div>
    </form>
</body>
</html>