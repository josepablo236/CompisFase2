# CompisFase2
Manejo de errores:

Manejamos los errores de dos maneras:
Al momento de encontrar un error nuestro analizador inserta el token que
produce ese error a una lista de errores, luego evalua si el
siguiente token pertenece a los terminales que reconoce el estado 0.
Si pertenecen a esta lista, se vacia la pila y la inicializamos de nuevo
en cero. También se vacía la lista de simbolos leidos y se decrementa el 
contador de tokens para que se vuelva a llamar a la función de parseo,
se dirija al estado cero y realice la acción correspondiente al token.
Si no pertenece a la lista de simbolos que reconoce el estado cero, el
analizador ignora todo lo que lea hasta encontrar el token esperado antes
de detectar el error.