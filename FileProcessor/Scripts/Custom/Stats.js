function fnGetStats() {
        $.ajax({
            url: ws + '/GetTotals',
            type: 'post',
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ uId: uId }),
            success: function (result) {
                var dt = JSON.parse(result.d);
                var tCalculations = dt.TotalCalculations;
                var tDuplicates = dt.TotalDuplicates;
                var tFiles = dt.TotalFiles;
                var tErred = dt.TotalErred;
               
                $("#tFiles").html(tFiles);
                $("#tErred").html(tErred);
                $("#tDuplicates").html(tDuplicates);
                $("#tCalculations").html(tCalculations);
            },
            error: function () {
                $("#tFiles").html("Unknown");
                $("#tErred").html("Unknown");
                $("#tDuplicates").html("Unknown");
                $("#tCalculations").html("Unknown");
            },
        });
}