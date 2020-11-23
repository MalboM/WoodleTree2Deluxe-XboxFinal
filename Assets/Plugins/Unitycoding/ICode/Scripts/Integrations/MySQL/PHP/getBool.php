<?php
	require_once("config.php");

	$auth_host = $GLOBALS['auth_host'];
	$auth_user = $GLOBALS['auth_user'];
	$auth_pass = $GLOBALS['auth_pass'];
	$auth_dbase = $GLOBALS['auth_dbase'];

 	$db = mysqli_connect($auth_host, $auth_user, $auth_pass,$auth_dbase) or die("Error " . mysqli_error($db));
	
	$key = mysqli_real_escape_string($db,$_POST['key']);

	$sql = mysqli_query($db,"SELECT value FROM bools WHERE name = '$key'");
	$row = mysqli_fetch_row($sql);

 	echo $row[0];
	mysqli_close($db);
?> 
 