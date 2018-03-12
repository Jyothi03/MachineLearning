Imports System.Math
Imports System
Imports System.Collections
Public Class genetic_code
    'variables declaration 
    ' population size equals to 200
    Dim SizePop As Integer = 200
    Dim Foldings As Integer
    ' 1st population
    Public population(200) As genotype
    'new population
    Public newpopulation(200) As genotype
    ' stores the protein structure 
    Dim StructureOfProtein As String
    'stores the input length
    Dim LengthOfProtein As Integer
    ' indexes of hydropohobic positions are stored in this array
    Dim HydrophobicPosition As Integer()
    'hydrophobic occurrences are stored
    Dim HydrophobicOccurences As Integer
    'current position
    Dim CurrentPositionNewPopulation As Integer
    Dim CompleteFitness As Integer = 0
    'stores the elite rate value
    Dim eliteRate As Decimal
    'stores the cross over 
    Dim crossOverRate As Decimal
    ' stores the mutation rate
    Dim RateOfMutation As Decimal
    Dim PositionOfMutationInNewPopulation As Integer
    Dim Generation As Integer = 0
    ' stores the max generations 
    Dim MaximumGenerations As Integer
    Public Class genotype
        Implements IComparable
        Public Fitness As Integer
        Public X(64) As Integer
        Public Y(64) As Integer
        'fitness is sorted by using the below function
        Public Function CompareTo(ByVal gene As Object) As Integer _
        Implements IComparable.CompareTo
            If CType(gene, genotype).Fitness < Me.Fitness Then
                Return 1
            ElseIf CType(gene, genotype).Fitness = Me.Fitness Then
                Return 0
            ElseIf CType(gene, genotype).Fitness > Me.Fitness Then
                Return -1
            End If
            Return Nothing
        End Function
    End Class
    'checks the input protein structure and saves the index value intlo array.

    Function Hyprophobicpositions()
        Dim HydrophobicIndex As Integer = 1
        LengthOfProtein = StructureOfProtein.Length
        HydrophobicPosition = New Integer(LengthOfProtein) {}
        HydrophobicOccurences = 0
        Dim hOccurence As Char() = StructureOfProtein.ToCharArray()
        For index = 1 To LengthOfProtein
            If (hOccurence(index - 1) = "h") Then
                HydrophobicPosition(HydrophobicIndex) = index
                HydrophobicIndex = HydrophobicIndex + 1
                HydrophobicOccurences = HydrophobicOccurences + 1
            End If
        Next index
        Return Nothing
    End Function

    Function Initialization()
        Dim i As Integer
        For i = 1 To SizeOfPopulation
            Folds = 0
            RandomOrientation(i)

            While (Folds = 0)
                RandomOrientation(i)
            End While
            population(i).Fitness = ComputeFitness(i)
            CompleteFitness = CompleteFitness + population(i).Fitness
        Next i
        Return Nothing
    End Function
    'computes the fitness of protein structure 
    Function ComputeFitness(n As Long) As Integer
        Dim isSequential As Integer
        Dim Fitness As Integer = 0
        Dim latticeDistance As Integer
        For i = 1 To HydrophobicOccurences - 1
            For j = i + 1 To HydrophobicOccurences
                isSequential = (Abs(HydrophobicPosition(i) - HydrophobicPosition(j))) '/*Not Sequential */
                If (isSequential <> 1) Then
                    latticeDistance = Abs(population(n).X(HydrophobicPosition(i)) - population(n).X(HydrophobicPosition(j))) + Abs(population(n).Y(HydrophobicPosition(i)) - population(n).Y(HydrophobicPosition(j)))
                    If (latticeDistance = 1) Then
                        Fitness = Fitness - 1
                    End If
                End If
            Next j
        Next i
        Return Fitness
    End Function
    'population of elite is calculated in this function  
    Private Sub caluclatingElitePopulation()
        newpopulation = New genotype(SizeOfPopulation) {}
        Dim elitePopulation As Integer
        elitePopulation = eliteRate * SizeOfPopulation
        Array.ConstrainedCopy(population, 1, newpopulation, 1, elitePopulation)
    End Sub
    'undergoes the crossover mutation 
    Private Sub CrossOverPopulation()
        Dim crossOverStartIndex As Integer = eliteRate * SizeOfPopulation + 1
        Dim crossOverLastIndex As Integer = crossOverRate * SizeOfPopulation + crossOverStartIndex - 1
        Dim crossOverPoint As Integer
        Dim i, j As Integer
        Dim maxEndPoint As Integer = LengthOfProtein - 3
        For Index = crossOverStartIndex To crossOverLastIndex
            CurrentPositionNewPopulation = Index
            Do Until i > 0
                i = ChromosomeSelectionUsingRoulettewheelSelection()
            Loop
            Do Until j > 0
                j = ChromosomeSelectionUsingRoulettewheelSelection()
            Loop
            newpopulation(CurrentPositionNewPopulation) = New genotype()
            Randomize()
            crossOverPoint = (maxEndPoint * Rnd() + 2)
            Dim Success As Integer = CrossOver(i, j, crossOverPoint)
            While Success = 0
                Do Until i > 0
                    i = ChromosomeSelectionUsingRoulettewheelSelection()
                Loop
                Do Until j > 0
                    j = ChromosomeSelectionUsingRoulettewheelSelection()
                Loop
                Randomize()
                crossOverPoint = (maxEndPoint * Rnd() + 2)
                Success = CrossOver(i, j, crossOverPoint)
            End While
        Next Index
    End Sub

    'This function will fill the remaining population in new population array
    Private Sub FillRemainingNewPopulation()
        Try
            Dim remainingNewPopulationStartIndex As Integer = eliteRate * SizeOfPopulation + crossOverRate * SizeOfPopulation + 1
            Array.ConstrainedCopy(population, remainingNewPopulationStartIndex, newpopulation, remainingNewPopulationStartIndex, SizeOfPopulation - remainingNewPopulationStartIndex + 1)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Function RandomOrientation(m As Long)

        Dim PreviousDirection, PresentDirection, i, temp1, temp2, temp3, X, Y, j, Flag, Step2 As Integer
        Dim a(4), Ax(4), Ay(4) As Integer

        '                                        3
        '             Select Direction as:     2 X 1
        '                                        4
        '
        population(m) = New genotype()
        Folds = 1
        population(m).X(1) = 0
        population(m).Y(1) = 0
        population(m).X(2) = 1
        population(m).Y(2) = 0
        PreviousDirection = 1


        For i = 3 To LengthOfProtein

            Select Case PreviousDirection
                Case 1
                    a(1) = 1
                    Ax(1) = 1
                    Ay(1) = 0
                    a(2) = 3
                    Ax(2) = 0
                    Ay(2) = 1
                    a(3) = 4
                    Ax(3) = 0
                    Ay(3) = -1
                Case 2
                    a(1) = 2
                    Ax(1) = -1
                    Ay(1) = 0
                    a(2) = 3
                    Ax(2) = 0
                    Ay(2) = 1
                    a(3) = 4
                    Ax(3) = 0
                    Ay(3) = -1
                Case 3
                    a(1) = 1
                    Ax(1) = 1
                    Ay(1) = 0
                    a(2) = 2
                    Ax(2) = -1
                    Ay(2) = 0
                    a(3) = 3
                    Ax(3) = 0
                    Ay(3) = 1
                Case 4
                    a(1) = 1
                    Ax(1) = 1
                    Ay(1) = 0
                    a(2) = 2
                    Ax(2) = -1
                    Ay(2) = 0
                    a(3) = 4
                    Ax(3) = 0
                    Ay(3) = -1
            End Select

            temp1 = Int(3 * Rnd() + 1)
            PresentDirection = temp1
            temp2 = 0
            temp3 = 0
            X = population(m).X(i - 1) + Ax(temp1)
            Y = population(m).Y(i - 1) + Ay(temp1)
            Flag = 0

            For j = 1 To i - 1
                If (X = population(m).X(j) And Y = population(m).Y(j)) Then
                    Flag = 1
                    GoTo MyJump1
                End If
            Next j

MyJump1:
            If (Flag = 1) Then
                Flag = 0
                Step2 = 6 - temp1
                Select Case Step2
                    Case 3
                        If Int(Rnd() * 2 + 1) = 1 Then
                            temp2 = 1
                        Else
                            temp2 = 2
                        End If
                    Case 4
                        If Int(Rnd() * 2 + 1) = 1 Then
                            temp2 = 1
                        Else
                            temp2 = 3
                        End If
                    Case 5
                        If Int(Rnd() * 2 + 1) = 1 Then
                            temp2 = 2
                        Else
                            temp2 = 3
                        End If
                End Select

                PresentDirection = temp2
                temp3 = 6 - (temp1 + temp2)
                X = population(m).X(i - 1) + Ax(temp2)
                Y = population(m).Y(i - 1) + Ay(temp2)

                For j = 1 To i - 1
                    If (X = population(m).X(j) And Y = population(m).Y(j)) Then
                        Flag = 1
                        GoTo MyJump2
                    End If
                Next j
MyJump2:
                If (Flag = 1) Then
                    Flag = 0
                    PresentDirection = temp3
                    X = population(m).X(i - 1) + Ax(temp3)
                    Y = population(m).Y(i - 1) + Ay(temp3)
                    For j = 1 To i - 1
                        If (X = population(m).X(j) And Y = population(m).Y(j)) Then
                            Flag = 1
                            Folds = 0
                            'GoTo MyJump3

                        End If
                    Next j
                End If
            End If
            PreviousDirection = a(PresentDirection)
            population(m).X(i) = population(m).X(i - 1) + Ax(PresentDirection)
            population(m).Y(i) = population(m).Y(i - 1) + Ay(PresentDirection)
        Next i
MyJump3:
        Return Nothing
    End Function
    ' the new population array is filled by remaining population 

    Private Function ChromosomeSelectionUsingRoulettewheelSelection() As Integer
        Randomize()
        Dim r1 As Integer
        Dim r2 As Integer
        Dim r3 As Integer
        Dim r4 As Integer
        Dim r5 As Integer
        Dim r6 As Integer
        Dim r7 As Integer
        Dim r8 As Integer
        Dim r9 As Integer
        Dim average As Integer
        r1 = Rnd() Mod SizeOfPopulation
        r2 = Rnd() Mod SizeOfPopulation
        r3 = Rnd() Mod SizeOfPopulation
        r4 = Rnd() Mod SizeOfPopulation
        r5 = Rnd() Mod SizeOfPopulation
        r6 = Rnd() Mod SizeOfPopulation
        r7 = Rnd() Mod SizeOfPopulation
        r8 = Rnd() Mod SizeOfPopulation
        r9 = Rnd() Mod SizeOfPopulation
        r2 = r2 + r1
        r3 = r3 + r2
        r4 = r4 + r3
        r5 = r5 + r4
        r6 = r6 + r5
        r7 = r7 + r6
        r8 = r8 + r7
        r9 = r9 + r8
        average = Math.Floor((r9) / 9)

        Return average
    End Function
    Function CrossOver(i As Long, j As Long, n As Integer) As Long

        Dim PrevDirection, k, z, p As Long
        Dim temp1, temp2, temp3, Collision, dx, dy, Step2 As Long
        Dim id As Long
        Dim a(0 To 4) As Long
        Dim Ax(0 To 4) As Long
        Dim Ay(0 To 4) As Long

        id = CurrentPositionNewPopulation

        '/* Detect Previous Direction */
        If (population(i).X(n) = population(i).X(n - 1)) Then
            p = population(i).Y(n - 1) - population(i).Y(n)
            If (p = 1) Then
                PrevDirection = 3
            Else
                PrevDirection = 4
            End If

        Else
            p = population(i).X(n - 1) - population(i).X(n)
            If (p = 1) Then
                PrevDirection = 1
            Else
                PrevDirection = 2
            End If
        End If


        Select Case PrevDirection
            Case 1
                Ax(1) = -1
                Ay(1) = 0
                Ax(2) = 0
                Ay(2) = 1
                Ax(3) = 0
                Ay(3) = -1
            Case 2
                Ax(1) = 1
                Ay(1) = 0
                Ax(2) = 0
                Ay(2) = 1
                Ax(3) = 0
                Ay(3) = -1
            Case 3
                Ax(1) = 1
                Ay(1) = 0
                Ax(2) = -1
                Ay(2) = 0
                Ax(3) = 0
                Ay(3) = -1

            Case 4
                Ax(1) = 1
                Ay(1) = 0
                Ax(2) = -1
                Ay(2) = 0
                Ax(3) = 0
                Ay(3) = 1
        End Select

        temp1 = Int(Rnd() * 3 + 1)

        newpopulation(id).X(n + 1) = population(i).X(n) + Ax(temp1)
        newpopulation(id).Y(n + 1) = population(i).Y(n) + Ay(temp1)
        Collision = 0

        dx = newpopulation(id).X(n + 1) - population(j).X(n + 1)
        dy = newpopulation(id).Y(n + 1) - population(j).Y(n + 1)

        For k = n + 1 To LengthOfProtein
            newpopulation(id).X(k) = population(j).X(k) + dx

            newpopulation(id).Y(k) = population(j).Y(k) + dy

            For z = 1 To n
                If ((newpopulation(id).X(k) = population(i).X(z)) And (newpopulation(id).Y(k) = population(i).Y(z))) Then
                    Collision = 1
                    GoTo MyOut1
                End If
            Next z
        Next k

MyOut1:
        If (Collision = 1) Then         '/* ======> Second try ==== */
            Collision = 0
            Step2 = 6 - temp1
            Select Case Step2
                Case 3
                    If Int(Rnd() * 2 + 1) = 1 Then
                        temp2 = 1
                    Else
                        temp2 = 2
                    End If

                Case 4
                    If Int(Rnd() * 2 + 1) = 1 Then
                        temp2 = 1
                    Else
                        temp2 = 3
                    End If

                Case 5
                    If Int(Rnd() * 2 + 1) = 1 Then
                        temp2 = 2
                    Else
                        temp2 = 3
                    End If
            End Select

            temp3 = 6 - (temp1 + temp2)
            newpopulation(id).X(n + 1) = population(i).X(n) + Ax(temp2)
            newpopulation(id).Y(n + 1) = population(i).Y(n) + Ay(temp2)
            dx = newpopulation(id).X(n + 1) - population(j).X(n + 1)
            dy = newpopulation(id).Y(n + 1) - population(j).Y(n + 1)

            For k = n + 1 To LengthOfProtein

                newpopulation(id).X(k) = population(j).X(k) + dx
                newpopulation(id).Y(k) = population(j).Y(k) + dy

                For z = 1 To n
                    If ((newpopulation(id).X(k) = population(i).X(z)) And (newpopulation(id).Y(k) = population(i).Y(z))) Then
                        Collision = 1
                        GoTo MyOut2
                    End If
                Next z
            Next k

MyOut2:
            If (Collision = 1) Then
                Collision = 0
                newpopulation(id).X(n + 1) = population(i).X(n) + Ax(temp3)
                newpopulation(id).Y(n + 1) = population(i).Y(n) + Ay(temp3)
                dx = newpopulation(id).X(n + 1) - population(j).X(n + 1)
                dy = newpopulation(id).Y(n + 1) - population(j).Y(n + 1)
                For k = n + 1 To LengthOfProtein
                    newpopulation(id).X(k) = population(j).X(k) + dx
                    newpopulation(id).Y(k) = population(j).Y(k) + dy
                    For z = 1 To n
                        If ((newpopulation(id).X(k) = population(i).X(z)) And (newpopulation(id).Y(k) = population(i).Y(z))) Then
                            Collision = 1
                            GoTo MyOut3
                        End If
                    Next z
                Next k
            End If '/* 3rd try if ends */
        End If '/* 2nd try if ends */

MyOut3:
        If Collision = 0 Then
            For k = 1 To n
                newpopulation(id).X(k) = population(i).X(k)
                newpopulation(id).Y(k) = population(i).Y(k)
            Next k
            CrossOver = 1
        End If

    End Function

    ' function will perform mutation
    Private Sub PerformMutation()

        Dim mutationPopulation As Integer = RateOfMutation * SizeOfPopulation
        Randomize()
        Dim geneToBeMutated As Integer = 199 * Rnd() + 1
        Randomize()
        Dim maxEndPoint As Integer = LengthOfProtein - 3
        Dim mutationPoint As Integer = maxEndPoint * Rnd() + 2
        Try
            Randomize()
            PositionOfMutationInNewPopulation = 189 * Rnd() + 11
            For index = 1 To mutationPopulation
                PositionOfMutationInNewPopulation = PositionOfMutationInNewPopulation
                Dim MutationStatus As Integer = Mutation(geneToBeMutated, mutationPoint)
                While MutationStatus = 0
                    geneToBeMutated = 199 * Rnd() + 1
                    mutationPoint = maxEndPoint * Rnd() + 2
                    MutationStatus = Mutation(geneToBeMutated, mutationPoint)
                End While
            Next
        Catch ex As Exception
            MessageBox.Show(geneToBeMutated + "geneToBeMutated" + mutationPoint + "mutationPoint" + PositionOfMutationInNewPopulation)
        End Try
    End Sub

    Function Mutation(i As Long, n As Integer) As Long
        Dim id As Long
        Dim a As Long
        Dim b As Long
        Dim A_Limit As Long
        Dim choice As Long
        Dim Collision As Long
        Dim k As Long
        Dim z As Long
        Dim p As Long
        Dim Ary(3) As Integer

        id = PositionOfMutationInNewPopulation

        ' possible rotations 90ß,180ß,270ß
        '           index       1   2    3
        '


        Ary(1) = 1
        Ary(2) = 2
        Ary(3) = 3
        A_Limit = 3

        a = population(i).X(n)          '/* (a, b) rotating point */
        b = population(i).Y(n)

        Do
            Collision = 0
            If (A_Limit > 1) Then
                Randomize()
                choice = Int(A_Limit * Rnd() + 1)
            Else
                choice = A_Limit
            End If


            p = Ary(choice)
            For k = choice To A_Limit - 1
                Ary(k) = Ary(k + 1)
            Next k

            A_Limit = A_Limit - 1

            For k = n + 1 To LengthOfProtein
                Select Case p

                    Case 1
                        newpopulation(id).X(k) = a + b - population(i).Y(k)       '/* X' = (a+b)-Y  */
                        newpopulation(id).Y(k) = population(i).X(k) + b - a       '/* Y' = (X+b)-a  */
                    Case 2
                        newpopulation(id).X(k) = 2 * a - population(i).X(k)       '/* X' = (2a - X) */
                        newpopulation(id).Y(k) = 2 * b - population(i).Y(k)       '/* Y' = (2b-Y)   */
                    Case 3
                        newpopulation(id).X(k) = population(i).Y(k) + a - b       '/* X' =  Y+a-b   */
                        newpopulation(id).Y(k) = a + b - population(i).X(k)       '/* Y' =  (a+b)-X */
                End Select

                For z = 1 To n

                    If ((newpopulation(id).X(k) = population(i).X(z)) And (newpopulation(id).Y(k) = population(i).Y(z))) Then
                        Collision = 1
                        GoTo MyJump
                    End If
                Next z
            Next k

            If (Collision = 0) Then
                A_Limit = 0
            End If
MyJump:
        Loop Until A_Limit = 0

        If (Collision = 0) Then
            For k = 1 To n
                newpopulation(id).X(k) = population(i).X(k)
                newpopulation(id).Y(k) = population(i).Y(k)
            Next k
            Mutation = 1
        Else
            Mutation = 0
        End If

    End Function

    ' function will compute the next generation
    Private Sub NextGeneration()
        Do
            myChart.Series("Protein Structure").Points.Clear()
            CompleteFitness = 0
            Array.ConstrainedCopy(newpopulation, 1, population, 1, SizeOfPopulation)
            For index = 1 To SizeOfPopulation
                population(index).Fitness = 0
                population(index).Fitness = ComputeFitness(index)
                CompleteFitness = CompleteFitness + population(index).Fitness
            Next

            'Sorting the Population based on the Fitness 
            Array.Sort(population)

            'Caluclate the elite population
            caluclatingElitePopulation()

            'CrossOver Population
            CrossOverPopulation()

            'Fill Remaining Population
            FillRemainingNewPopulation()

            ' perform Mutation
            PerformMutation()
            Generation = Generation + 1
            TextBox6.Text = population(1).Fitness
            TextBox6.Enabled = False
            myChart.Series("Protein Structure").BorderWidth = 5
            myChart.ChartAreas(0).AxisX.Interval = 1
            myChart.ChartAreas(0).AxisY.Interval = 1
            For index = 1 To StructureOfProtein.Length
                myChart.Series("Protein Structure").Points.AddXY(population(1).X(index), population(1).Y(index))
            Next
            For c = 0 To LengthOfProtein - 1
                'HydropPhobic Positions is represented in Black Color
                If HydrophobicPosition.Contains(c + 1) Then
                    myChart.Series("Protein Structure").Points(c).MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
                    myChart.Series("Protein Structure").Points(c).MarkerSize = 10
                    myChart.Series("Protein Structure").Points(c).MarkerColor = Color.Black
                Else
                    'HydropPhilic Positions is represented in Red Color
                    myChart.Series("Protein Structure").Points(c).MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
                    myChart.Series("Protein Structure").Points(c).MarkerSize = 10
                    myChart.Series("Protein Structure").Points(c).MarkerColor = Color.Red
                End If
            Next
            TextBox7.Text = Generation
            TextBox7.Enabled = False
            myChart.Refresh()
            TextBox6.Refresh()
            TextBox7.Refresh()
            System.Threading.Thread.Sleep(30)
        Loop Until Generation = MaximumGenerations
        Return
    End Sub

    'This function will start  to execute after accepting the inputs dynamically
    Private Sub StartExecution()
        Hyprophobicpositions()
        Initialization()
        Array.Sort(population)
        caluclatingElitePopulation()
        CrossOverPopulation()
        FillRemainingNewPopulation()
        PerformMutation()
        NextGeneration()
    End Sub

    ' This function will get executed when the start button is clicked only after accepting the inputs dynamically
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Generation = 0
            StructureOfProtein = TextBox1.Text
            eliteRate = TextBox2.Text
            crossOverRate = TextBox3.Text
            RateOfMutation = TextBox4.Text
            MaximumGenerations = Val(TextBox5.Text)
            StartExecution()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    ' This function will get executed when the reset button is clicked 
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.ResetText()
        TextBox2.ResetText()
        TextBox3.ResetText()
        TextBox4.ResetText()
        TextBox5.ResetText()
        TextBox6.ResetText()
        TextBox7.ResetText()
        myChart.Series("Protein Structure").Points.Clear()
        Generation = 0
    End Sub




End Class
