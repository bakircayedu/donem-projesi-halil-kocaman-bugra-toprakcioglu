function ReportForm() {
    document.getElementById("rapor").style.display = "inline-block";
}
function RaporSuccess() {
    document.getElementById("rapor").style.display = "none";
    alert('Rapor Tarafımıza iletilmiştir..');
}
function RaporCancel() {
    document.getElementById("rapor").style.display = "none";
   
}
function FavoriEkle() {
    alert('Makale Favorilere Eklendi');
    document.getElementById("favori").style.color = "#f31d2a";
}