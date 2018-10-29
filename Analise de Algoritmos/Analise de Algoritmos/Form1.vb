Imports System.Threading

Public Class Form1
    Private t1, t2, t3, t4 As Thread
    Private vetor As New List(Of Integer)
    Dim cronometro As New Stopwatch

    '' BOTAO GERAR VETOR
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RichTextBox1.Clear()
        ToolStripStatusLabel2.Text = "Gerando vetor..."
        RichTextBox1.AppendText("Gerando vetor...")
        cnt = 0
        vetor = New List(Of Integer)

        t1 = New Thread(AddressOf gerarVetor)
        t2 = New Thread(AddressOf gerarVetor)
        t3 = New Thread(AddressOf gerarVetor)
        t4 = New Thread(AddressOf gerarVetor)
        t1.Priority = ThreadPriority.Highest
        t2.Priority = ThreadPriority.Highest
        t3.Priority = ThreadPriority.Highest
        t4.Priority = ThreadPriority.Highest
        Dim valor As Integer = (NumericUpDown1.Value / 4) - 1

        t1.Start(valor)
        t2.Start(valor)
        t3.Start(valor)
        t4.Start(valor)
        cronometro.Reset()
        cronometro.Start()

        'BackgroundWorker1.RunWorkerAsync(NumericUpDown1.Value)
        TabControl1.Enabled = False

    End Sub

    '' FUNCAO GERAR VETOR
    Private Sub gerarVetor(v As Integer)
        Dim r As New Random
        Dim max As Integer = v * 4
        Dim vetorTemp(v) As Integer

        For index = 0 To v
            vetorTemp(index) = r.Next(max)
        Next

        Me.Invoke(Sub() receberVetor(vetorTemp))
    End Sub

    '' FUNCAO JUNTAR VETORES DAS THREADS
    Dim cnt As Integer = 0
    Private Sub receberVetor(ByVal v() As Integer)
        vetor.AddRange(v.ToList)
        cnt += 1
        ToolStripStatusLabel2.Text = "Vetor: " & cnt & "/" & 4

        If cnt = 4 Then
            RichTextBox1.Clear()
            cronometro.Stop()

            ToolStripStatusLabel2.Text = "Vetor Gerado. Primeiros 100 itens:"

            For index = 0 To (100)
                RichTextBox1.AppendText(vetor(index) & vbNewLine)
            Next

            'ToolStripStatusLabel2.Text = "Vetor Gerado alatóriamente, apresentando os primeiros 100 itens."

            Dim ts As TimeSpan = cronometro.Elapsed
            ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
            TabControl1.Enabled = True

        End If

    End Sub

    Private Sub TabPage2_enter(sender As Object, e As EventArgs) Handles TabPage2.Enter
        If vetor.Count = 0 Then
            MsgBox("Primeiro gere um vetor.")
            TabControl1.SelectedIndex = 0
        End If
    End Sub

    Private Sub TabPage3_enter(sender As Object, e As EventArgs) Handles TabPage3.Enter
        If vetor.Count = 0 Then
            MsgBox("Primeiro gere um vetor e teste os algoritmos.")
            TabControl1.SelectedIndex = 0
        End If
    End Sub

    '' BOTAO TESTAR ALGORITMO
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If vetor.Count > 10000 And ComboBox1.SelectedIndex > 1 Then
            If MsgBox("Há um numero alto de itens no vetor, pode demorar, deseja continuar mesmo assim?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return
            End If
        End If

        Select Case ComboBox1.SelectedIndex
            Case -1
                MsgBox("Selecione um algoritmo")
                ComboBox1.Focus()
            Case 0  ' Busca Linear
                If String.IsNullOrEmpty(TextBox1.Text) Then
                    MsgBox("Informe um número para buscar no vetor.")
                    TextBox1.Focus()
                Else
                    testarAlgoritmo(0, TextBox1.Text)
                End If
            Case 1  ' Busca Binária
                If String.IsNullOrEmpty(TextBox1.Text) Then
                    MsgBox("Informe um número para buscar no vetor.")
                    TextBox1.Focus()
                Else
                    testarAlgoritmo(1, TextBox1.Text)
                End If
            Case 2  ' Ordenação por seleção
                testarAlgoritmo(2)
            Case 3  ' Ordenação por Inserção
                testarAlgoritmo(3)
            Case 4  ' Ordenação por Intercalação
                testarAlgoritmo(4)
            Case 5  ' Ordenação por Separação
                testarAlgoritmo(5)
            Case 6  ' ?
                MsgBox("Erro ao selecionar algoritmo.")

            Case Else
                MsgBox("Erro ao selecionar algoritmo.")
        End Select
    End Sub

    '' TESTAR ALGORITMO
    Private Sub testarAlgoritmo(ByVal algoritmo As Integer, Optional buscar As String = "")
        TabControl1.Enabled = False
        RichTextBox2.AppendText("Vetor: " & vetor.Count & " itens" & vbNewLine)

        If algoritmo = 0 Or algoritmo = 1 Then
            If Not xAlgBusca.Contains(Format(vetor.Count, "###,###,###,##0")) Then
                'xAlgBusca.Add(vetor.Count)
                xAlgBusca.Add(Format(vetor.Count, "###,###,###,##0"))
            End If

        ElseIf algoritmo = 2 Or algoritmo = 3 Or algoritmo = 4 Or algoritmo = 5 Then
            If Not xAlgOrd.Contains(Format(vetor.Count, "###,###,###,##0")) Then
                xAlgOrd.Add(Format(vetor.Count, "###,###,###,##0"))
            End If

        End If


        Select Case algoritmo
            Case 0  ' Busca Linear
                ToolStripStatusLabel2.Text = "Algoritmo: Busca Linear"
                RichTextBox2.AppendText("Algoritmo: Busca Linear" & vbNewLine)
                cronometro.Reset()
                cronometro.Start()

                '' Algoritmo:
                For index = 0 To vetor.Count - 1
                    If vetor(index) = buscar Then
                        RichTextBox2.AppendText("Busca Linear: Encontrou '" & buscar & "' na posição: " & index & vbNewLine)
                    End If
                Next

                cronometro.Stop()
                Dim ts As TimeSpan = cronometro.Elapsed
                yTempoBuscaLin.Add(ts.TotalMilliseconds)

                ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
                RichTextBox2.AppendText("Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds) & vbNewLine)
            Case 1  ' Busca Binária
                ToolStripStatusLabel2.Text = "Algoritmo: Busca Binária"
                RichTextBox2.AppendText("Algoritmo: Busca Binária" & vbNewLine)
                cronometro.Reset()
                cronometro.Start()

                '' Algoritmo:
                If vetor.BinarySearch(buscar) Then
                    RichTextBox2.AppendText("Busca Binária: Encontrou '" & buscar & "'" & vbNewLine)
                End If

                cronometro.Stop()
                Dim ts As TimeSpan = cronometro.Elapsed
                yTempoBuscaBin.Add(ts.TotalMilliseconds)

                ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
                RichTextBox2.AppendText("Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds) & vbNewLine)
            Case 2  ' Ordenação por seleção
                ToolStripStatusLabel2.Text = "Algoritmo: Ordenação por seleção"
                RichTextBox2.AppendText("Algoritmo: Ordenação por seleção" & vbNewLine)
                Dim vetorTemp() = vetor.ToArray
                Dim vetorTempList As New List(Of Integer)
                vetorTempList.AddRange(vetor)
                cronometro.Reset()
                cronometro.Start()

                '' Algoritmo:
                For index = 0 To vetorTemp.Length - 1
                    Dim menor = index
                    For i = menor + 1 To vetorTemp.Length - 1
                        If vetorTemp(i) < vetorTemp(menor) Then
                            menor = i
                        End If
                    Next
                    If menor <> index Then
                        Dim t As Integer = vetorTemp(index)
                        vetorTemp(index) = vetorTemp(menor)
                        vetorTemp(menor) = t
                    End If
                Next

                cronometro.Stop()

                RichTextBox2.AppendText("Ordenação por seleção: Vetor Ordenado. " & vbNewLine)

                vetorTempList.Sort()
                If CheckBox1.Checked Then
                    For Each v In vetorTempList
                        RichTextBox2.AppendText(v & "-")
                    Next
                End If
                RichTextBox2.AppendText("" & vbNewLine)

                Dim ts As TimeSpan = cronometro.Elapsed
                yTempoOrdSel.Add(ts.TotalMilliseconds)
                ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
                RichTextBox2.AppendText("Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds) & vbNewLine)
            Case 3  ' Ordenação por Inserção
                ToolStripStatusLabel2.Text = "Algoritmo: Ordenação por Inserção"
                RichTextBox2.AppendText("Algoritmo: Ordenação por Inserção" & vbNewLine)
                Dim vetorTemp() = vetor.ToArray
                Dim vetorTempList As New List(Of Integer)
                vetorTempList.AddRange(vetor)
                cronometro.Reset()
                cronometro.Start()

                '' Algoritmo:
                Dim j, key, i As Integer

                For j = 1 To vetorTemp.Count - 1
                    key = vetorTemp(j)
                    For i = (j - 1) To 0 Step -1
                        If vetorTemp(i) > key Then : vetorTemp(i + 1) = vetorTemp(i) : End If
                    Next
                    vetorTemp(i + 1) = key
                Next

                cronometro.Stop()
                RichTextBox2.AppendText("Ordenação por seleção: Vetor Inserção. " & vbNewLine)

                vetorTempList.Sort()
                If CheckBox1.Checked Then
                    For Each v In vetorTempList
                        RichTextBox2.AppendText(v & "-")
                    Next
                End If
                RichTextBox2.AppendText("" & vbNewLine)

                Dim ts As TimeSpan = cronometro.Elapsed
                yTempoOrdIns.Add(ts.TotalMilliseconds)
                ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
                RichTextBox2.AppendText("Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds) & vbNewLine)
            Case 4  ' Ordenação por Intercalação
                ToolStripStatusLabel2.Text = "Algoritmo: Ordenação por Intercalação"
                RichTextBox2.AppendText("Algoritmo: Ordenação por Intercalação" & vbNewLine)
                Dim vetorTempList As New List(Of Integer)
                vetorTempList.AddRange(vetor)
                Dim vetorTemp() = vetor.ToArray
                cronometro.Reset()
                cronometro.Start()

                '' Algoritmo:
                Dim troca As Boolean = True
                Dim aux As Integer
                While troca
                    troca = False
                    For index = 0 To vetorTemp.Count - 2
                        If vetorTemp(index) > vetorTemp(index + 1) Then
                            aux = vetorTemp(index)
                            vetorTemp(index) = vetorTemp(index + 1)
                            vetorTemp(index + 1) = aux
                            troca = True
                        End If
                    Next
                End While

                cronometro.Stop()

                RichTextBox2.AppendText("Ordenação por Intercalação: Vetor Ordenado. " & vbNewLine)

                vetorTempList.Sort()
                If CheckBox1.Checked Then
                    For Each v In vetorTempList
                        RichTextBox2.AppendText(v & "-")
                    Next
                End If
                RichTextBox2.AppendText("" & vbNewLine)

                Dim ts As TimeSpan = cronometro.Elapsed
                yTempoOrdInt.Add(ts.TotalMilliseconds)
                ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
                RichTextBox2.AppendText("Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds) & vbNewLine)
            Case 5  ' Ordenação por Separação
                ToolStripStatusLabel2.Text = "Algoritmo: Ordenação por Separação"
                RichTextBox2.AppendText("Algoritmo: Ordenação por Separação" & vbNewLine)
                Dim vetorTemp() = vetor.ToArray
                Dim vetorTempList As New List(Of Integer)
                vetorTempList.AddRange(vetor)
                cronometro.Reset()
                cronometro.Start()

                '' Algoritmo: "QuickSort"
                Try
                    quickSort(vetorTemp, 0, vetorTemp.Length - 1)
                Catch ex As Exception
                    MsgBox("Erro no ordenamento. " & ex.Message)
                End Try
                cronometro.Stop()
                RichTextBox2.AppendText("Ordenação por Separação: Vetor Ordenado. " & vbNewLine)

                vetorTempList.Sort()
                If CheckBox1.Checked Then
                    For Each v In vetorTempList
                        RichTextBox2.AppendText(v & "-")
                    Next
                End If
                RichTextBox2.AppendText("" & vbNewLine)

                Dim ts As TimeSpan = cronometro.Elapsed
                yTempoOrdSep.Add(ts.TotalMilliseconds)
                ToolStripStatusLabel2.Text = "Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
                RichTextBox2.AppendText("Tempo decorrido: " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds) & vbNewLine)
            Case Else
                MsgBox("Algoritmo desconhecido.")
        End Select

        TabControl1.Enabled = True
        carregaGrafico1()
        carregaGrafico2()

        'Return True
    End Sub

    Dim xAlgBusca As New List(Of String)
    Dim yTempoBuscaLin As New List(Of Double)
    Dim yTempoBuscaBin As New List(Of Double)
    Dim xAlgOrd As New List(Of String) 'As String = {"Seleção", "Inserção", "Intercalação", "Separação"}
    Dim yTempoOrdSel As New List(Of Double)
    Dim yTempoOrdIns As New List(Of Double)
    Dim yTempoOrdInt As New List(Of Double)
    Dim yTempoOrdSep As New List(Of Double)

    '' TESTAR TODOS ALGORITMOS
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        '' Busca
        If CheckBox2.Checked Then
            testarAlgoritmo(0, "1")
            testarAlgoritmo(1, "1")
        End If

        '' Ordenação:
        If CheckBox3.Checked Then
            testarAlgoritmo(2)
            testarAlgoritmo(3)
            testarAlgoritmo(4)
            testarAlgoritmo(5)
        End If

        carregaGrafico1()   '' Busca
        carregaGrafico2()   '' Ordenação

    End Sub

    Private Sub carregaGrafico1()
        'ver se tem dados em todos vetores
        If xAlgBusca.Count = 0 Then : Return : End If
        If xAlgBusca.Count <> yTempoBuscaLin.Count Then : Return : End If
        If xAlgBusca.Count <> yTempoBuscaBin.Count Then : Return : End If

        With Chart1
            ''Define o tipo de gráfico
            .Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Column
            .Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Column

            ''Define o texto da legenda 
            '.Series(0).LegendText = "Vendas"

            ''Define o titulo do eixo y , sua fonte e a cor
            .ChartAreas(0).AxisY.Title = "Tempo (ms)"
            .ChartAreas(0).AxisY.TitleFont = New Font("Calibri", 12, FontStyle.Bold)
            .ChartAreas(0).AxisY.TitleForeColor = Color.Blue

            ''Define o titulo do eixo x , sua fonte e a cor
            .ChartAreas(0).AxisX.Title = "Quantidade de Itens no Vetor"
            .ChartAreas(0).AxisX.TitleFont = New Font("Calibri", 12, FontStyle.Bold)
            .ChartAreas(0).AxisX.TitleForeColor = Color.Blue
            .ChartAreas(0).AxisX.LabelStyle.Angle = -45
            '.ChartAreas(0).AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount
            .ChartAreas(0).AxisX.Interval = 1

            ''Define a paleta de cores usada
            '.Palette = DataVisualization.Charting.ChartColorPalette.Bright

            ''Vincula os dados ao gráfico
            .Series(0).Points.DataBindXY(xAlgBusca, yTempoBuscaLin)
            .Series(1).Points.DataBindXY(xAlgBusca, yTempoBuscaBin)

            ''Exibe os valores nos eixos
            .Series(0).IsValueShownAsLabel = True
            .Series(0).BorderWidth = 2

            .Series(1).IsValueShownAsLabel = True
            .Series(1).BorderWidth = 2

            '' Pode mostrar fora da caixa padrão
            .Series(0).SmartLabelStyle.AllowOutsidePlotArea = DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes
            .Series(0).SmartLabelStyle.Enabled = True
            .Series(1).SmartLabelStyle.AllowOutsidePlotArea = DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes
            .Series(1).SmartLabelStyle.Enabled = True

            ''Habilta a exibição 3D
            .ChartAreas(0).Area3DStyle.Enable3D = True

        End With
    End Sub

    Private Sub carregaGrafico2()
        'ver se tem dados em todos vetores
        If xAlgOrd.Count = 0 Then : Return : End If
        If xAlgOrd.Count <> yTempoOrdSel.Count Then : Return : End If
        If xAlgOrd.Count <> yTempoOrdIns.Count Then : Return : End If
        If xAlgOrd.Count <> yTempoOrdInt.Count Then : Return : End If
        If xAlgOrd.Count <> yTempoOrdSep.Count Then : Return : End If

        With Chart2
            ''Define o tipo de gráfico
            .Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Column
            .Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Column
            .Series(2).ChartType = DataVisualization.Charting.SeriesChartType.Column
            .Series(3).ChartType = DataVisualization.Charting.SeriesChartType.Column

            ''Define o texto da legenda 
            '.Series(0).LegendText = "Vendas"

            ''Define o titulo do eixo y , sua fonte e a cor
            .ChartAreas(0).AxisY.Title = "Tempo (ms)"
            .ChartAreas(0).AxisY.TitleFont = New Font("Calibri", 12, FontStyle.Bold)
            .ChartAreas(0).AxisY.TitleForeColor = Color.Blue

            ''Define o titulo do eixo x , sua fonte e a cor
            .ChartAreas(0).AxisX.Title = "Ordenação"
            .ChartAreas(0).AxisX.TitleFont = New Font("Calibri", 12, FontStyle.Bold)
            .ChartAreas(0).AxisX.TitleForeColor = Color.Blue
            .ChartAreas(0).AxisX.LabelStyle.Angle = -45
            '.ChartAreas(0).AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount
            .ChartAreas(0).AxisX.Interval = 1

            ''Define a paleta de cores usada
            '.Palette = DataVisualization.Charting.ChartColorPalette.Bright

            ''Vincula os dados ao gráfico
            .Series(0).Points.DataBindXY(xAlgOrd, yTempoOrdSel)
            .Series(1).Points.DataBindXY(xAlgOrd, yTempoOrdIns)
            .Series(2).Points.DataBindXY(xAlgOrd, yTempoOrdInt)
            .Series(3).Points.DataBindXY(xAlgOrd, yTempoOrdSep)

            ''Exibe os valores nos eixos
            .Series(0).IsValueShownAsLabel = True
            .Series(1).IsValueShownAsLabel = True
            .Series(2).IsValueShownAsLabel = True
            .Series(3).IsValueShownAsLabel = True

            ''Habilta a exibição 3D
            .ChartAreas(0).Area3DStyle.Enable3D = True
            .ChartAreas(0).Area3DStyle.IsClustered = True
            .ChartAreas(0).Area3DStyle.LightStyle = DataVisualization.Charting.LightStyle.Realistic

        End With
    End Sub

    '' QUICK SORT:
    Private Sub quickSort(ByRef vetorTemp() As Integer, inicio As Integer, fim As Integer)
        If inicio < fim Then
            Dim posicaoPivo As Integer = separar(vetorTemp, inicio, fim)
            quickSort(vetorTemp, inicio, posicaoPivo - 1)
            quickSort(vetorTemp, posicaoPivo + 1, fim)
        End If

    End Sub
    Private Function separar(ByRef vetorInt() As Integer, inicio As Integer, fim As Integer) As Object
        Dim pivo As Integer = vetorInt(inicio)
        Dim i As Integer = inicio + 1
        Dim f As Integer = fim

        While i < f
                If vetorInt(i) <= pivo Then
                    i += 1
                ElseIf pivo < vetorInt(f) Then
                    f -= 1
                Else
                    Dim troca As Integer = vetorInt(i)
                    vetorInt(i) = vetorInt(f)
                    vetorInt(f) = troca
                    i += 1
                    f -= 1
                End If
            End While
            vetorInt(inicio) = vetorInt(f)
        vetorInt(f) = pivo

        Return f
    End Function

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim r As New Random
        Dim vetorTemp(e.Argument) As Integer

        For index = 0 To e.Argument
            vetorTemp(index) = r.Next(NumericUpDown1.Value)
            'RichTextBox1.AppendText(vetorTemp(index) & vbNewLine)
        Next

        e.Result = vetorTemp
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

        Try
            Dim vetorTemp() As Integer = e.Result

            RichTextBox1.Clear()

            For index = 0 To (vetorTemp.Length - 1)
                RichTextBox1.AppendText(vetorTemp(index) & vbNewLine)
            Next

        Catch ex As Exception
            MsgBox("Erro. " & ex.Message)
        End Try

        TabControl1.Enabled = True

    End Sub


End Class
