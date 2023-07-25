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
Guide() {
    echo "1) El comando "$(tput setaf 4)"<run>"$(tput sgr0)" te permite ejecutar el Moogle"
    echo " "
    echo "2) El comando "$(tput setaf 4)"<report>"$(tput sgr0)" te permite Compilar y generar el PDF del Informe del Moogle (Latex) que se encuentra en la carpeta Informe"
    echo " "
    echo "3) El comando "$(tput setaf 4)"<slides>"$(tput sgr0)" te permite Compilar y generar el PDF de la Presentacion del Moogle (Latex) que se encuentra en la carpeta Presentacion"
    echo " "
    echo "4) El comando "$(tput setaf 4)"<show_report>"$(tput sgr0)" te permite mostrar el PDF Informe del Moogle, y si este no ha sido generado, lo genera y luego lo muestra"
    echo "    - Este comando tiene la utilidad de que puede ser ejecutado con el lector de PDF que desee, solo basta con pasarselo como parametro de la siguiente forma: "
    echo "    "$(tput setaf 4)"<script_name.sh> <show_report> <lector_a_usar>"$(tput sgr0)
    echo "    En el caso de que no le pase ningun lector como parametro, se abrira con un lector de PDF por defecto"
    echo " "
    echo "5) El comando "$(tput setaf 4)"<show_slides>"$(tput sgr0)" te permite mostrar el PDF Presentacion del Moogle, y si este no ha sido generado, lo genera y luego lo muestra"
    echo "    - Este comando tiene la utilidad de que puede ser ejecutado con el lector de PDF que desee, solo basta con pasarselo como parametro de la siguiente forma: "
    echo "    "$(tput setaf 4)"<script_name.sh> <show_slides> <lector_a_usar>"$(tput sgr0)
    echo "    En el caso de que no le pase ningun lector como parametro, se abrira con un lector de PDF por defecto"
    echo " "
    echo "6) El comando "$(tput setaf 4)"<clean>"$(tput sgr0)" te permite borrar los archivos auxiliares que se crean cuando se compilan y se general los PDF del Informe y la Presentacion"
    echo " "
    echo "7) El comando "$(tput setaf 4)"<ZIP>"$(tput sgr0)" te permite comprimir el Informe,la Presentacion y la carpeta del Proyecto, para usarlo solo debe seguir con las instrucciones que se muestran al darle al comando"
}


echo "
▒█▀▀▀█ ▒█▀▀█ ▒█▀▀█ ▀█▀ ▒█▀▀█ ▀▀█▀▀ 
░▀▀▀▄▄ ▒█░░░ ▒█▄▄▀ ▒█░ ▒█▄▄█ ░▒█░░ 
▒█▄▄▄█ ▒█▄▄█ ▒█░▒█ ▄█▄ ▒█░░░ ░▒█░░"
# opciones
OPTIONS="run report slides show_report show_slides clean ZIP Guide"

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
