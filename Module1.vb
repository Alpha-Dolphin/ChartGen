Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Runtime.CompilerServices

Module Module1
    Sub Main(args As String())
        Dim output As New Chart
        Dim form As New Form
        Dim intArgs As Integer() = New Integer(args.Length) {}
        Dim TwoDdata As Integer()() = New Integer(args.Length / 2 - 1)() {}
        Try
            intArgs = Array.ConvertAll(args, Function(str As String) Integer.Parse(str))
            For i As Integer = 0 To intArgs.Length - 1
                'Can only fill jagged arrays with defined arrays, rather than a collection of same-type objects
                Dim temp As Integer() = New Integer() {intArgs.ElementAt(i), intArgs(i + 1)}
                TwoDdata(i / 2) = temp
                ' So i increments by two
                i += 1
            Next
        Catch ex As FormatException
            Console.WriteLine("Invalid number format.")
            Console.ReadLine()
            Exit Sub
        Catch ex As IndexOutOfRangeException
            Console.WriteLine("Data points must be in two-dimensions")
            Console.ReadLine()
            Exit Sub
        End Try
        output = ChartGen(TwoDdata, {"X", "Y"})
        form.Controls.Add(output)
        form.Show()
        Console.WriteLine("Press Enter Key to Exit..")
        Console.ReadLine()
    End Sub

    Public Function ChartGen(data As Integer()(), Axises As String())

        Dim Chart1 As New Chart

        ' Add a chart area to the chart
        Chart1.ChartAreas.Add("ChartArea1")

        ' Set the chart area size and location
        Chart1.ChartAreas(0).Position.X = 50
        Chart1.ChartAreas(0).Position.Y = 50
        Chart1.ChartAreas(0).Position.Width = 50
        Chart1.ChartAreas(0).Position.Height = 50

        ' Set the chart title
        Chart1.Titles.Add("Data Visualization")

        ' Create a new series and add it to the chart
        Dim series As New Series With {
            .Name = "Data",
            .ChartType = SeriesChartType.Point
        }

        ' Add data to the series
        For Each element As Integer() In data
            series.Points.Add(element.ElementAt(0), element.ElementAt(1))
        Next element

        Chart1.Series.Add(series)

        ' Set the axis labels
        Chart1.ChartAreas(0).AxisX.Name = Axises(0)
        Chart1.ChartAreas(0).AxisY.Name = Axises(1)
        Return Chart1
    End Function

End Module