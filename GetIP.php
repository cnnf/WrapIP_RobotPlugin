<?php

$user_online = $_GET['username'].".txt";
if($user_online == '.txt'){
	exit('���ݴ��� �봫��username����');
}

$time =(int)date('s',time()); //��ǰ����
if ($time == 30){ //��ǰ�����30 ɾ������txt�ļ� ���⻺���ļ����� Ҳ��������������~
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

touch($user_online);//���û�д��ļ����򴴽�
$timeout = 30;//����û������,��Ϊ����
$user_arr = file_get_contents($user_online);
$user_arr = explode('#',rtrim($user_arr,'#'));print_r($user_arr);
$temp = array();
foreach($user_arr as $value){
$user = explode(",",trim($value));
if (($user[0] != getenv('REMOTE_ADDR')) && ($user[1] > time())) {//������Ǳ��û�IP��ʱ��û�г�ʱ����뵽������
array_push($temp,$user[0].",".$user[1]);
}
}
array_push($temp,getenv('REMOTE_ADDR').",".(time() + ($timeout)).'#'); //���汾�û�����Ϣ
$user_arr = implode("#",$temp);
//д���ļ�
$fp = fopen($user_online,"w");
flock($fp,LOCK_EX); //flock() ������NFS�Լ�������һЩ�����ļ�ϵͳ����������
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