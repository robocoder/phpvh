<?php
sqlite_unbuffered_query("handle", "SELECT * FROM table WHERE field='$_GET[query]'");
?>