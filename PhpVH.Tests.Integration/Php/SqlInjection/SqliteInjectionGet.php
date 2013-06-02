<?php
sqlite_query("handle", "SELECT * FROM table WHERE field='$_GET[query]'");
?>