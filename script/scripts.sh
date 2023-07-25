#!/bin/bash

# Funciones
run() {
    echo "Ejecutando el proyecto..."
    cd ..
    dotnet watch run --project MoogleServer
    cd script
}

report() {
    echo "Compilando y generando el PDF del informe..."
    cd ..
    cd Informe
    pdflatex Informe.tex
    cd ..
    cd script
}

slides() {
    echo "Compilando y generando el PDF de la presentación..."
    cd ..
    cd Presentacion
    #chmod u+w Presentacion_Moogle.tex
    pdflatex Presentacion_Moogle.tex
    cd ..
    cd script
}

show_report() {
    cd ..
    cd Informe
    if [ ! -f Informe.pdf ]; then
        echo "Compilando y generando el PDF del informe..."
        pdflatex Informe.tex
    fi
    echo "Visualizando el informe..."
    if [ -z "$1"]; then
        xdg-open Informe.pdf
    else
        $1 Informe.pdf
        return 1
    fi
    cd ..
    cd script
}

show_slides() {
    cd ..
    cd Presentacion
    if [ ! -f Presentacion_Moogle.pdf]; then
        slides
    fi
    echo "Visualizando la presentación..."
    if [ -z "$1"]; then
        xdg-open Presentacion_Moogle.pdf
    else
        $1 Presentacion_Moogle.pdf
        return 1
    fi
    cd ..
    cd script
}

clean() {
    echo "Eliminando los ficheros auxiliares..."
    cd ..
    cd Informe
    find . ! -name '*.tex' ! -name '*.png' ! -name '*.jpg' -type f -delete
    cd ..
    cd Presentacion
    find . ! -name '*.tex' -type f -delete
    cd ..
    cd script
}

# opciones
OPTIONS="run report slides show_report show_slides clean"

# Ejecución del script

OPT=$1
if [ "$OPT" = "" ]; then
    echo "Sintaxys se uso: $0 <opcion>"
    echo ""
    echo "Opciones:"
    for i in $OPTIONS; do
        echo "  $i"
    done
    exit 1
fi

"$@"
