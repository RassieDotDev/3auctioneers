<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="WebApplication4.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>

     <p id="curbid">R <br /><br /></p>
                          <script src="~/Scripts/jquery-1.10.2.js"></script>
                        <script src="~/Scripts/jquery.signalR-2.2.3.js"></script>
      
                        <script type="text/javascript">
                            $(function () {
                                
                                var con = $.hubConnection();
                                var hub = con.createHubProxy('LiveBid');
                                hub.on('onBidRecorded', function(i) {
                                    $('#curbid').text(i);
                                });
                                con.start(function () {
                                    hub.invoke('addBid');
                                    
                                });
                            })


                        </script>
</body>
</html>
