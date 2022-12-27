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
            Next i
        Catch ex As FormatException
            Console.WriteLine("Invalid number format.")
            Console.ReadLine()
            Exit Sub
        Catch ex As IndexOutOfRangeException
            Console.WriteLine("Data points must be in two-dimensions")
            Console.ReadLine()
            Exit Sub
        End Try
        output = ChartGen(TwoDdata, 5.0, "Sample", {"X", "Y"})
        form.Controls.Add(output)
        form.Show()
        Console.WriteLine("Press Enter Key to Exit..")
        Console.ReadLine()
    End Sub

    Public Function ChartGen(data As Integer()(), numberingSpacing As Decimal, Title As String, Axises As String())

        Dim Chart1 As New Chart

        ' Add a chart area
        Chart1.ChartAreas.Add("ChartArea1")

        ' Set the chart area size and location
        Chart1.ChartAreas(0).Position.X = 50
        Chart1.ChartAreas(0).Position.Y = 50
        Chart1.ChartAreas(0).Position.Width = 50
        Chart1.ChartAreas(0).Position.Height = 50

        ' Set the chart title
        Chart1.Titles.Add(Title)

        ' Create a new series and add it to the chart
        Dim series As New Series With {
            .Name = "Data",
            .ChartType = SeriesChartType.Point
        }

        ' Add data to the series
        If (numberingSpacing <> 0F) Then

            Dim minX As Integer = (2 ^ 15) - 1
            Dim maxX As Integer = ((2 ^ 15) - 1) * -1
            Dim minY As Integer = (2 ^ 15) - 1
            Dim maxY As Integer = ((2 ^ 15) - 1) * -1.0

            For Each element As Integer() In data
                series.Points.Add(element.ElementAt(0), element.ElementAt(1))
                If (element.ElementAt(0) < minX) Then minX = element.ElementAt(0)
                If (element.ElementAt(0) > maxX) Then maxX = element.ElementAt(0)
                If (element.ElementAt(1) < minY) Then minY = element.ElementAt(1)
                If (element.ElementAt(1) > maxY) Then maxY = element.ElementAt(1)
            Next element

            ' Set the interval of the major tick marks to numberingSpacing
            Chart1.ChartAreas(0).AxisX.MajorTickMark.Interval = numberingSpacing
            Chart1.ChartAreas(0).AxisY.MajorTickMark.Interval = numberingSpacing

            ' Set the interval of the minor tick marks to numberingSpacing / 2
            Chart1.ChartAreas(0).AxisX.MinorTickMark.Interval = numberingSpacing / 2
            Chart1.ChartAreas(0).AxisY.MinorTickMark.Interval = numberingSpacing / 2

            ' Set the interval offset if needed
            Chart1.ChartAreas(0).AxisX.MajorTickMark.IntervalOffset = numberingSpacing
            Chart1.ChartAreas(0).AxisY.MajorTickMark.IntervalOffset = numberingSpacing

            ' Set the numbers on the x-axis
            Dim intervalX As Double = (maxX - minX) / numberingSpacing
            For i As Integer = minX To maxX Step intervalX
                Chart1.ChartAreas(0).AxisX.CustomLabels.Add(i - (intervalX / 2), i + (intervalX / 2), i.ToString())
            Next i

            ' Set the numbers on the y-axis
            Dim intervalY As Double = (maxY - minY) / numberingSpacing
            For i As Integer = minY To maxY Step intervalY
                Chart1.ChartAreas(0).AxisY.CustomLabels.Add(i - (intervalY / 2), i + (intervalY / 2), i.ToString())
            Next i
        Else
            For Each element As Integer() In data
                series.Points.Add(element.ElementAt(0), element.ElementAt(1))
            Next element
        End If

        Chart1.Series.Add(series)

        ' Set the axis labels
        If (Axises.Length = 2) Then
            Chart1.ChartAreas(0).AxisX.Name = Axises(0)
            Chart1.ChartAreas(0).AxisY.Name = Axises(1)
        Else
            Console.WriteLine("Axises array of wrong length")
            Throw New Exception
        End If

        Return Chart1
    End Function

End Module