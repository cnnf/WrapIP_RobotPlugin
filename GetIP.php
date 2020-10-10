<?php

$user_online = $_GET['username'].".txt";
if($user_online == '.txt'){
	exit('数据错误 请传递username数据');
}

$time =(int)date('s',time()); //当前秒数
if ($time == 30){ //当前秒等于30 删除所有txt文件 避免缓存文件过多 也可以用其他方法~
	foreach( glob('*.txt') as $file )
 {
	 if($file != 'com_loxve_probe.txt'){
		 unlink($file);
	 }

 }
}

$img_url = base64_decode($_GET['url']);
$id=rand(1,13);
$image[1]='http://ip.lance.moe/images/amamiya.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[2]='http://ip.lance.moe/images/angel.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[3]='http://ip.lance.moe/images/lovelive.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[4]='http://ip.lance.moe/images/misaka.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[5]='http://ip.lance.moe/images/rika.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[6]='http://ip.lance.moe/images/titan.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[7]='http://ip.lance.moe/images/yosuga.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[8]='http://ip.lance.moe/images/yuno.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[9]='http://ip.lance.moe/images/shimakaze.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[10]='http://ip.lance.moe/images/vocaloid.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[11]='http://ip.lance.moe/images/shana.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[12]='http://ip.lance.moe/images/eva.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';
$image[13]='http://ip.lance.moe/images/gosec.png?wd=5Y%2Bv5piv5oiR55qE5L2N572u5bey6KKr6LCB5Y%2BW5Luj77yf';

touch($user_online);//如果没有此文件，则创建
$timeout = 30;//秒内没动作者,认为掉线
$user_arr = file_get_contents($user_online);
$user_arr = explode('#',rtrim($user_arr,'#'));print_r($user_arr);
$temp = array();
foreach($user_arr as $value){
$user = explode(",",trim($value));
if (($user[0] != getenv('REMOTE_ADDR')) && ($user[1] > time())) {//如果不是本用户IP并时间没有超时则放入到数组中
array_push($temp,$user[0].",".$user[1]);
}
}
array_push($temp,getenv('REMOTE_ADDR').",".(time() + ($timeout)).'#'); //保存本用户的信息
$user_arr = implode("#",$temp);
//写入文件
$fp = fopen($user_online,"w");
flock($fp,LOCK_EX); //flock() 不能在NFS以及其他的一些网络文件系统中正常工作
fputs($fp,$user_arr);
flock($fp,LOCK_UN);
fclose($fp);
if ($img_url != ""){
	header("location:".$img_url);
}
else{
	header("location:$image[$id]");
}
?>