function listPages_SelectionChanged(){
    var listPages = document.getElementById("listPages");
    //alert("Default.aspx?page=" + listPages.selectedIndex);
    window.location =../Default.aspx?page=" + listPages.selectedIndex;
}