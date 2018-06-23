<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="KreyGasm.MainForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>MNN</title>
    <link href="Momo.png" rel="icon" type="image/ico"/>
    <link type="text/css" href="style.css" rel="stylesheet" />
    <script type="text/javascript" src="sketchpad.js"></script>
</head>
<body style="margin:0;padding:0;height:100vh;">
    <div id="particles-js" style="width: 100%;height: 100%;background-color: black;z-index: -1;"> </div>
    <form id="MainForm" runat="server" enableviewstate="False" role="banner">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/Neuron.asmx" />
            </Services>
        </asp:ScriptManager>
        <script type="text/javascript" src="particles.js"></script>
        <script type="text/javascript" src="app.js"></script>
        <div style="height:100px;"></div>
        <div id="MainDiv" style="height: auto; padding : 50px; text-align : center;">
            <div class="MainTitle">Mnist Neural Network</div>
            <br />
            <div id="result" ></div>
            <canvas width="280" height="280" id='canvas' accesskey="canvas"></canvas>
            <canvas width="28" height="28" id='thumbnail' accesskey="canvas"></canvas>
            <br />
            <br />
            <input type="button" class="main_but" id="Cl_But" value="Очистить" />
            <input type="button" class="main_but" id="Sub_But" value="Распознать" />
            <br />
            <br />
            <span style="color:white;margin-right:25px;font-family:'Courier New';font-size:x-large"> Результат : </span>
            <div id="title"></div>
        </div>
    </form>
    
</body>
</html>
