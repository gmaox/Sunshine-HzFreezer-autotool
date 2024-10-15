# 串流自动冻结小工具
通过监听网络端口48000，实现串流暂停自动冻结，连接自动解冻<p>
由于python keyboard库无法实现alt+win+c这种类型组合键（bug？），请手动修改并输入可行的组合键<p>
**部分游戏（如原神）需要 *管理员身份* 运行此工具才能正常冻结！！**
# 使用教程
视频教程↓<p>
<a href="http://b23.tv/ZOkEsk7">【串流小工具】自动冻结工具使用教程</a><p>
文字教程↓<p>
0--首先你得先安装雪藏。。<p>
雪藏发布链接：https://github.com/superDMS/HsFreezer-Hidden-in-the-snow-/<p>
1--配置快捷键，使程序正常工作（理论上两个快捷键一致即可工作，但由于前文所说bug，我推荐将雪藏冻结快捷键设置为图中两个）
![屏幕截图 2024-09-27 180148](https://github.com/user-attachments/assets/60702f5f-5a28-49ed-9e21-ce57ebea512c)
2--确保后台任务栏有心海图标即可。若快捷键设置正确，当使用moonlight连接时，雪藏若弹出解冻程序提示即可正常使用！<p>
![371861266-d91ddec5-096d-440f-b618-d71ab8246c11](https://github.com/user-attachments/assets/a9fc074f-a849-4097-8c86-87b962c3a242)<p>
目前有三种使用方法（点击展开）<p>

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

默认管理员启动更方便~
![屏幕截图 2024-10-05 213147](https://github.com/user-attachments/assets/91a78059-1b69-462c-b605-fa90ba618de1)

</details>
<p>
该程序有什么用？：串流异常（如掉线）时雪藏美好瞬间，让您不错过游戏的精彩时刻。以及带来掌机般的便捷休眠体验。<p><hr>
目前只支持sunshine，英伟达shine是不能用的（应该能用？但我没法运行shine来调试所以不能用<p>
网易gv经测试得出端口是在串流断开后过一大会才会释放，并且每次连接端口号都不一样，故不做适配
