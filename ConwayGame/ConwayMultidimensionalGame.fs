﻿module ConwayMultidimensionalGame

type Cell =
    | Line of int
    | Surface of int * int
    | Space of int * int * int
    | Spacetime of int * int * int * int

let neighbours cell =
    match cell with
    | Line(a) -> [ for i in a-1..a+1 do 
                   if not (i = a) then 
                       yield Line(i) ]
    | Surface(a,b) -> [ for i in a-1..a+1 do 
                        for j in b-1..b+1 do 
                        if not (i = a && j = b) then 
                            yield Surface(i,j) ]
    | Space(a,b,c) -> [ for i in a-1..a+1 do 
                        for j in b-1..b+1 do 
                        for k in c-1..c+1 do 
                        if not (i = a && j = b && k = c) then 
                            yield Space(i,j,k) ]
    | Spacetime(a,b,c,d) -> [ for i in a-1..a+1 do 
                              for j in b-1..b+1 do 
                              for k in c-1..c+1 do 
                              for l in d-1..d+1 do 
                              if not (i = a && j = b && k = c && l = d) then 
                                  yield Spacetime(i,j,k,l) ]

let isAlive pattern cell =
    pattern |> List.exists ((=) cell)

let aliveNeighbours pattern cell =
    neighbours cell
    |> List.filter (isAlive pattern)

let survives pattern cell =
    aliveNeighbours pattern cell |> List.length |> fun x -> x >= 2 && x <= 3

let reproducible pattern cell =
    aliveNeighbours pattern cell |> List.length = 3

let allDeadNeighbours pattern =
    pattern
    |> List.collect neighbours
    |> Set.ofList |> Set.toList
    |> List.filter (not << isAlive pattern)

let nextGeneration pattern =
    List.append
        (pattern |> List.filter (survives pattern))
        (allDeadNeighbours pattern |> List.filter (reproducible pattern))

let rec patternFromTuples cellFromTuple pattern  =
    match pattern with
    | [] -> []
    | head :: tail -> (cellFromTuple head) :: (patternFromTuples cellFromTuple tail)

let rec tuplesFrompattern tupleFromCell pattern =
    match pattern with
    | [] -> []
    | head :: tail -> (tupleFromCell head) :: (tuplesFrompattern tupleFromCell tail)

let nextGenerationWithConversion pattern cellFromTuple tupleFromCell =
    let typedpattern = pattern |> patternFromTuples cellFromTuple
    typedpattern
    |> nextGeneration
    |> tuplesFrompattern tupleFromCell
