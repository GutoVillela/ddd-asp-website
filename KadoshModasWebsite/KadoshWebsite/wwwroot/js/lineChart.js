function LoadLineChart(chartId, chartMainLabel, urlToFetchData, loadingSpinnerId) {
    $.ajax({
        method: 'GET',
        url: urlToFetchData,
        beforeSend: function () {
            ShowLineChartLoadingSpinner(loadingSpinnerId);
        }
    }).done(function (lineChartData) {
        LoadInfoToLineChart(chartId, chartMainLabel, lineChartData);
    }).fail(function (error) {
        ShowErrorMessage(error.responseText);
    }).always(function () {
        HideLineChartLoadingSpinner(loadingSpinnerId);
    });
}

function LoadInfoToLineChart(chartId, chartMainLabel, lineChartData) {
    const context = document.getElementById(chartId).getContext('2d');

    const chartData = {
        labels: lineChartData.labels,
        datasets: [
            {
                label: 'Vendas no dia', // TODO Receive this as a parameter
                data: lineChartData.data,
                borderColor: 'red',
                backgroundColor: 'rgba(139,0,0, .5)',
                pointStyle: 'circle',
                pointRadius: 10,
                pointHoverRadius: 15
            }
        ]
    };

    const myChart = new Chart(context, {
        type: 'line',
        data: chartData,
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: (ctx) => chartMainLabel,
                }
            }
        }
    });
}



function ShowLineChartLoadingSpinner(loadingSpinnerId) {
    $('#' + loadingSpinnerId.id).show();
}

function HideLineChartLoadingSpinner(loadingSpinnerId) {
    $('#' + loadingSpinnerId).hide();
}

function ShowErrorMessage(errorMessage) {
    if (typeof ToastErrorMessage != "function") {
        console.error('The script "toastMessage.js" is missing on the parent View.');
        console.error('Error: ' + errorMessage);
    }
    else
        ToastErrorMessage(errorMessage);
}