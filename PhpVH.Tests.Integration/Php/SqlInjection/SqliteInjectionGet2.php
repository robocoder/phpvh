<?php
sqlite_exec("handle", "SELECT * FROM table WHERE field='$_GET[query]'");
?>