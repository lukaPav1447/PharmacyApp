var ukupanTotal = 0;
var kategorijaId = 0;
var total2 = 0;

function minusElem(element, id) {
    var cijena = "#cijenaProizvoda_" + id;
    var elem = "#p" + id;
    var kelem = "#k" + id;
    var celem = "#c" + id;

    if ($(elem).text() === id + "") {
        $(kelem).text(parseInt($(kelem).text()) - 1)
        var kolicina = parseInt($(kelem).text());
        console.log(kolicina )
        if (kolicina <= 0) {
            console.log('U petlji smo')
            $(element).closest('tr').remove()
        }
        $(celem).text((parseFloat(($(cijena).text())) * kolicina).toFixed(2));
        total2 -= parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }
    else {
        $("#bodyTabele").append(text);
        total2 += parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }
}

function plusElem(element, id) {
    var cijena = "#cijenaProizvoda_" + id;
    var elem = "#p" + id;
    var kelem = "#k" + id;
    var celem = "#c" + id;

    if ($(elem).text() === id + "") {
        $(kelem).text(parseInt($(kelem).text()) + 1)
        var kolicina = parseInt($(kelem).text());
        console.log(kolicina)
        if (kolicina <= 0) {
            console.log('U petlji smo')
            $(element).closest('tr').remove()
        }
        $(celem).text((parseFloat(($(cijena).text())) * kolicina).toFixed(2));
        total2 += parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }
    else {
        $("#bodyTabele").append(text);
        total2 += parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }
}

function clearElem(element, id) {
    var cijena = "#cijenaProizvoda_" + id;
    var elem = "#p" + id;
    var kelem = "#k" + id;
    var celem = "#c" + id;

    if ($(elem).text() === id + "") {
        var kolicina = parseInt($(kelem).text());        
        total2 -= parseFloat($(cijena).text() * kolicina);

        $("#total").text("Total: " + total2.toFixed(2) + " KM")
        $(element).closest('tr').remove();
    }
    else {
        $("#bodyTabele").append(text);
        total2 += parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }
}

function ucitavanjePodataka(searchQuery, idKategorije) {
    $.ajax({
        type: "GET",
        url: '/ProductForOrderItems/SearchByName',
        data: { searchQuery: searchQuery, idKategorije: idKategorije },
        success: function (data) {
            console.log(data)
            $.each(data, function (index, item) {
                var isDisabled = item.quantity === 0;
                // Do something with each item in the response
                var text = `<div class="col-md-4 "> 
                <div id="`+ item.productid + `" class=" m-1 p-2 rounded-3 mojeDugme kategorija-` + item.categoryid + `" ${isDisabled ? 'disabled-div' : ''} onclick='ispisiArtikalNaRacun(this);'>
                        <div class="text-center imeProizvoda" id="imeProizvoda_`+ item.productid + `">` + item.name + `</div>
                        <div class="text-center minusElem" id="cijenaProizvoda_`+ item.productid + `" minusElem>` + item.baseprice + `</div>
                    </div>
                </div>`
                $("#listaProizvoda").append(text);
            });
        }
    });
}
$(document).ready(function () {

    $("#listaProizvoda").empty();
    ucitavanjePodataka('', 0)

});
function pretragaLijekova(e) {
    console.log("ispis nekis" + e.value)
    var searchQuery = e.value;
    $("#listaProizvoda").empty();

    ucitavanjePodataka(searchQuery, kategorijaId)

}
function ispisiArtikalNaRacun(e) {
    var id = $(e).attr("id");
    var naziv = "#imeProizvoda_" + id;
    var cijena = "#cijenaProizvoda_" + id;
    var komada = 1;

    var text = '<tr><td class="d-none" id="p' + id + '" >' + id + '</td><td>' + $(naziv).text() + '</td><td><a href="#" id = "minusElem' + id + '"  onclick="minusElem(this, ' + id +
        ')"><i class="fa-solid px-2 fa-circle-minus text-danger text-warning"></i></a></td><td id="k' + id + '" class="d-flex p-2"> ' + komada + '</td><td><a href="#" id = "plusElem' + id + '"  onclick="plusElem(this, ' + id +
        ')"><i class="fa-solid fa-circle-plus text-success"></i></a></td><td id="c' + id + '" class="d-flex p-2">' + $(cijena).text() +
        '</td><td><a href="#"' + id + '" onclick="clearElem(this, ' + id + ')"><i class="fa-regular fa-circle-xmark text-warning"></i></td></tr>'
    var elem = "#p" + id;
    var kelem = "#k" + id;
    var celem = "#c" + id;

    if ($(elem).text() === id + "") {
        $(kelem).text(parseInt($(kelem).text()) + 1)
        var kolicina = parseInt($(kelem).text());
        $(celem).text((parseFloat(($(cijena).text())) * kolicina).toFixed(2));
        total2 += parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }

    else {
        $("#bodyTabele").append(text);
        total2 += parseFloat($(cijena).text());
        $("#total").text("Total: " + total2.toFixed(2) + " KM")
    }
}
function pretragaPoKategoriji(idKategorije) {

    $("#listaProizvoda").empty();
    kategorijaId = idKategorije;
    var searchQuery = $("#pretraga").val() == undefined ? '' : $("#pretraga").val() ;
    ucitavanjePodataka(searchQuery, idKategorije);
    console.log(idKategorije)
}
    $(".mojeDugme").click(function (e) {
        var id = $(this).attr("id");
        var naziv = "#imeProizvoda_" + id;
        var cijena = "#cijenaProizvoda_" + id;
        console.log("Moj id" + id)
        var komada = 1;
        
        var text = '<tr><td class="d-none" id="p' + id + '" >' + id + '</td><td>' + $(naziv).text() + '</td><td><a href="#" id = "minusElem' + id + '"  onclick="minusElem(this, ' + id +
            ')"><i class="fa-solid px-2 fa-circle-minus text-danger text-warning"></i></a></td><td id="k' + id + '" class="d-flex p-2"> ' + komada + '</td><td><a href="#" id = "plusElem' + id + '"  onclick="plusElem(this, ' + id +
            ')"><i class="fa-solid fa-circle-plus text-success"></i></a></td><td id="c' + id + '" class="d-flex p-2">' + $(cijena).text() +
            '</td><td><a href="#"' + id + '" onclick="clearElem(this, ' + id + ')"><i class="fa-regular fa-circle-xmark text-warning"></i></td></tr>'
        var elem = "#p" + id;
        var kelem = "#k" + id;
        var celem = "#c" + id;

        if ($(elem).text() === id + "") {
            $(kelem).text(parseInt($(kelem).text()) + 1)
            var kolicina = parseInt($(kelem).text());
            $(celem).text((parseFloat(($(cijena).text())) * kolicina).toFixed(2));
            total2 += parseFloat($(cijena).text());
            $("#total").text("Total: " + total2.toFixed(2) + " KM")
        }

        else {
            $("#bodyTabele").append(text);
            total2 += parseFloat($(cijena).text());
            $("#total").text("Total: " + total2.toFixed(2) + " KM")
        }

    });


function preuzmiPodatkeIzTabele() {
    var narudzba = {
        "Orderdate": new Date(),
        "Totalamount": total2.toFixed(2),
        "Orderitems": []
    };

    $('#bodyTabele tr').each(function () {
        var red = $(this);
        var orderitemid = red.find('td:nth-child(1)').text();
        var kolicina = parseInt(red.find('td:nth-child(4)').text());
        var cijena = parseFloat(red.find('td:nth-child(6)').text());

        var stavkaNarudzbe = {
            "Productid": orderitemid,
            "Quantity": kolicina,
            "Price": cijena
        };

        narudzba.Orderitems.push(stavkaNarudzbe);

        //narudzba.Orderitems.forEach(function (stavka) {
        //    console.log('Kolicina:', stavka.Quantity);
        //    console.log('Product ID:', stavka.Productid);
        //    console.log('Cijena:', stavka.Price);
        //});
    });

    if (narudzba.Orderitems.length > 0) {

        $.ajax({
            url: '/Order/SnimiNarudzbu',
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(narudzba),
            contentType: 'application/json',
            success: function (response) {
                // Uspješna obrada odgovora
                $("#bodyTabele").empty();
                total2 = 0;
                $("#total").text("Total: " + total2.toFixed(2) + " KM")
                alert("Narudzba uspjesno potvrdjena")
            },
            error: function (xhr, status, error) {
                // Greška prilikom slanja podataka
                console.log(error);
                console.log("Ovdje smo...")
            }
        });
    }
    else {
        alert("Nema artikala u korpi...")
    }
}

