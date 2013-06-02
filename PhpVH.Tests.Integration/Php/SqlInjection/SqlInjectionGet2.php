<?php
mysql_unbuffered_query("SELECT * FROM table WHERE field='$_GET[query]'");
?>