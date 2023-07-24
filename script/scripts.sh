#!/bin/bash

# Definición de variables

#PROYECTO="/media/cami/Datos/PROGRAMACION/PROYECTO/moogle-main"
#INFORME="/media/cami/Datos/PROGRAMACION/PROYECTO/moogle-main/Informe"
#PRESENTACION="/media/cami/Datos/PROGRAMACION/PROYECTO/moogle-main/Presentacion"


# Funciones

function run() {
    echo "Ejecutando el proyecto..."
    cd ..
    dotnet watch run --project MoogleServer
    cd script
}

function report() {
    echo "Compilando y generando el PDF del informe..."
    cd ..
    cd Informe
    pdflatex Informe.tex
    cd ..
    cd script
}

function slides() {
    echo "Compilando y generando el PDF de la presentación..."
    cd ..
    cd Presentacion
    chmod u+w Presentacion_Moogle.tex
    pdflatex Presentacion_Moogle.tex
    cd ..
    cd script
}

function show_report() {
    echo "Visualizando el informe..."
    cd ..
    cd Informe
    if [ ! -f Informe.pdf ]; then
        echo "Compilando y generando el PDF del informe..."
        pdflatex Informe.tex
    fi
    evince Informe.pdf
    cd ..
    cd script
}

function show_slides() {
    echo "Visualizando la presentación..."
    cd ..
    cd Presentacion
    if [ ! -f Presentacion_Moogle.pdf]; then
        slides
    fi
    evince Presentacion_Moogle.pdf
    cd ..
    cd script
}

function clean() {
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

case "$OPT" in
    run)
        run
        ;;
    report)
        report
        ;;
    slides)
        slides
        ;;
    show_report)
        show_report
        ;;
    show_slides)
        show_slides
        ;;
    clean)
        clean
        ;;
    *)
        echo "Opción no válida"
        exit 1
        ;;
esac