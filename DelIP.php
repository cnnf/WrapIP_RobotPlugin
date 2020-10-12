<?php
function get_allfiles($path,&$files) {
    if(is_dir($path)){
        $dp = dir($path);
        while ($file = $dp ->read()){
            if($file !="." && $file !=".."){
                //get_allfiles($path."/".$file, $files);				
				if (substr($file,-4)=='.txt'){
                   get_allfiles($file, $files);
                }
            }
        }
        $dp ->close();
    }
    if(is_file($path)){
        $files[] =  $path;
    }
}
   
function get_filenamesbydir($dir){
    $files =  array();
    get_allfiles($dir,$files);
    return $files;
}
   


$action = $_GET['action'];
$dir=dirname(__FILE__);
//echo $dir;
if(!$action)
{
	echo "Please enter an action.";
}
else
{	
	if($action == "DelAll")
	{	
       $files=glob("*.txt");
       foreach($files as $file)
       unlink($file);
    }
	else if($action == "FindAll")
	{
		$filenames = get_filenamesbydir($dir);
        foreach ($filenames as $value) 
		{
            echo $value ."\r\n";
        }
	}
}	



?>