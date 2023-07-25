#!/bin/bash

# Funciones

run() {
    echo " "
    echo -e "\033[36m"Ejecutando el proyecto..."\033[0m"
    echo " "
    cd ..
    dotnet watch run --project MoogleServer
    cd script
}

report() {
    echo " "
    echo -e "\033[36m"Compilando y generando el PDF del informe..."\033[0m"
    echo " "
    cd ..
    cd Informe
    pdflatex Informe.tex
    cd ..
    cd script
}

slides() {
    echo " "
    echo -e "\033[36m"Compilando y generando el PDF de la presentación..."\033[0m"
    #echo "Compilando y generando el PDF de la presentación..."
    echo " "
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
        echo " "
        echo -e "\033[36m"Compilando y generando el PDF del informe..."\033[0m"
        #echo "Compilando y generando el PDF del informe..."
        echo " "
        pdflatex Informe.tex
    fi
    echo " "
    echo -e "\033[36m"Visualizando el informe..."\033[0m"
    #echo "Visualizando el informe..."
    echo " "
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
        echo " "
        echo -e "\033[36m"Compilando y generando el PDF de la presentación..."\033[0m"
        #echo "Compilando y generando el PDF de la presentación..."

        echo " "
        slides
    fi
    echo " "
    echo -e "\033[36m"Visualizando la presentacion.."\033[0m"
    echo " "
    #echo "Visualizando la presentación..."
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
    echo " "
    echo -e "\033[36m"Eliminando los ficheros auxiliares..."\033[0m"
    #echo "Eliminando los ficheros auxiliares..."
    echo " "
    cd ..
    cd Informe
    find . ! -name '*.tex' ! -name '*.png' ! -name '*.jpg' -type f -delete
    cd ..
    cd Presentacion
    find . ! -name '*.tex' -type f -delete
    cd ..
    cd script
}

ZIP() {
    echo " "
    echo "Escriba <Informe> si desea comprimir el informe"
    echo "Escriba <Presentacion> si desea comprimir la Presentacion"
    echo "Escriba <Moogle> si desea comprimir la Carpeta entera del Moogle"
    echo " "
    read archivo

    if [ "$archivo" = "Informe" ]; then
        echo " "
        echo "Comprimiendo $archivo.pdf en $archivo.zip..."
        cd ..
        cd Informe
        zip "$archivo.zip" "$archivo.pdf"
        cd ..
        echo "Archivo comprimido $archivo.zip creado exitosamente."
        cd script
    elif [ "$archivo" = "Presentacion" ]; then
        echo "Comprimiendo $archivo.pdf en $archivo.zip..."
        cd ..
        cd Presentacion
        zip "$archivo.zip" "$archivo.pdf"
        cd ..
        echo "Archivo comprimido $archivo.zip creado exitosamente."
        cd script
    elif [ "$archivo" = "Moogle" ]; then
        echo "Comprimiendo $archivo.pdf en $archivo.zip..."
        cd ..
        cd ..
        zip -r "$archivo.zip" "Moogle-main"
        cd ..
        echo "Archivo comprimido $archivo.zip creado exitosamente."
        cd script
    fi

}

echo "
▒█▀▀▀█ ▒█▀▀█ ▒█▀▀█ ▀█▀ ▒█▀▀█ ▀▀█▀▀ 
░▀▀▀▄▄ ▒█░░░ ▒█▄▄▀ ▒█░ ▒█▄▄█ ░▒█░░ 
▒█▄▄▄█ ▒█▄▄█ ▒█░▒█ ▄█▄ ▒█░░░ ░▒█░░"
# opciones
OPTIONS="run report slides show_report show_slides clean ZIP"

# Ejecución del script

OPT=$1
if [ "$OPT" = "" ]; then
    echo " "
    echo "Sintaxys se uso: $0 <opcion>"
    echo ""
    echo "Opciones:"
    for i in $OPTIONS; do
        echo "  $i"
    done
    exit 1
fi

"$@"
