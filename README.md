# 串流自动冻结小工具
通过监听sunshine的UDP端口48000，实现串流暂停自动冻结，连接自动解冻<p>
由于python keyboard库无法实现alt+win+c这种类型组合键（bug？），请手动修改并输入可行的组合键<p>
# 使用教程
视频教程↓<p>
<a href="http://b23.tv/ZOkEsk7">【串流小工具】自动冻结工具使用教程</a><p>
文字教程↓<p>
0--首先你得先安装雪藏。。<p>
雪藏链接：https://github.com/superDMS/HsFreezer-Hidden-in-the-snow-/<p>
<details>
<summary>
（点击展开剩余教程）
</summary><p>
1--打开程序设置，配置快捷键（理论上两个快捷键一致即可工作，但由于前文所说问题，我推荐将雪藏冻结快捷键设置为图中两个，你也可以自行尝试有哪些复杂快捷键支持）然后重启雪藏
    
![屏幕截图 2024-09-27 180148](https://github.com/user-attachments/assets/60702f5f-5a28-49ed-9e21-ce57ebea512c)
<p>2--确保后台任务栏程序存在。若快捷键设置正确，当使用moonlight连接时，雪藏若弹出解冻程序提示即可正常使用！<p>
    
![373626189-a9fc074f-a849-4097-8c86-87b962c3a242](https://github.com/user-attachments/assets/fd7b0670-6a5d-44b3-8241-0e0f4bf3c4c0)

<p>
点击任务栏图标可以进行状态切换：运行↔暂停<p>
    
![屏幕截图 2024-11-09 224053](https://github.com/user-attachments/assets/10c36809-7002-4537-a916-eccec38210af)

</details>
<p>
<h2>启动方式</h2>
<p>
<details> 
    <summary>方法一：推荐在程序设置中开启开机自启本工具，可避免方法二在登录界面时无法启动串流的问题</summary>

![屏幕截图 2024-10-14 121149](https://github.com/user-attachments/assets/88b88c1e-ab78-4901-82d5-ef326f1139ad)

</details>
<details> 
    <summary>方法二：将该工具添加到sunshine，让他随着串流同步运行</summary>
  
![屏幕截图 2024-09-27 175802](https://github.com/user-attachments/assets/b940b781-97ec-4b58-a3be-69e147da7ecf)
![屏幕截图 2024-10-04 194448](https://github.com/user-attachments/assets/ce23789c-dc0d-409b-b4f0-b8872cafad89)

</details>
<details> 
    <summary>方法三：每次想用的时候手动点击启动</summary>

创建快捷方式或加入到游戏列表中

</details>
<p>
    
<h2>使用技巧</h2>

可以将办公应用（如浏览器）加入雪藏黑名单中，退出串流就不会冻结该应用。<p>若希望短期内某个应用在退出串流后不冻结，可在退出前先win+d回到桌面。<p>仅全屏冻结模式还会改变解冻的逻辑，如果你对每次串流雪藏弹出来空空如也的提示所困扰，可以尝试在程序设置中开启仅全屏冻结

<h2>程序卸载</h2>

在程序设置中关闭开机自启，然后删除整个freezeautotool文件夹即可
<h2>程序理念</h2>
串流异常（如掉线）时雪藏美好瞬间，让您不错过游戏的精彩时刻。以及带来掌机般的便捷休眠体验。<hr>
目前只支持sunshine，英伟达shield是不能用的（应该能用？但我没法运行shield来调试所以不能用<p>
网易gv经测试得出端口是在串流断开后过一大会才会释放，并且每次连接端口号都不一样，故不做适配
<details> 
    <summary>
        关于故障排查
    </summary>

在后台任务栏图标右键菜单有个调试按钮，点开会弹出一个记录程序运行状况的黑窗口。以下是正常工作的记录
<p>
    
![image](https://github.com/user-attachments/assets/bb6977e0-2035-41de-8114-9f74ba4929a1)
若出现无法工作的状况，可对比调试窗口是否和上图记录一样
<p><hr>
案例分析一：用户安装了GameStream IPv6 Forwarder插件导致系统端口占用程序无法正常识别<p>
分析过程：先打开调试窗口，发现端口被pid0所占用<p>

![image](https://github.com/user-attachments/assets/da534e17-b327-4e43-93cb-f24b43f38bfb)
然后win+r，输入cmd，在终端中输入netstat -ano | findstr 48000，发现有两个程序在占用端口<p>

![image](https://github.com/user-attachments/assets/e3bcf511-18f5-4c64-91e6-3fd6dd85b723)
接着在任务管理器中查询这两个pid，发现一个是sunshine另一个是这个插件<p>

![image](https://github.com/user-attachments/assets/93bb6d0b-00b5-4853-9ad7-d7c0ff70e467)
<p>
解决方案：关闭该软件开机自启，停止使用该插件。（这个插件是为英伟达串流服务的，sunshine并不需要）



</details>
