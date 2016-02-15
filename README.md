# Escaleras
Esta es una prueba de progracioón realizada usando como objetivo el desarrollo del famoso juego de mesa escalera.

El juego se trata del jugador que primero llegue a la meta (a la casilla 100) recorriendo el tablero haciendo uso de las escaleras y evitando a toda costa bajar por (en este caso) las culebras.

Para este juego, y dadas las mecanicas y la velocidad (pausado) de desarrallo del juego, decidí prevalecer dentro del juego un diseño dominado por Event Driven Design por algunas razones, sobre todo para evitar el cross wiring. Esto lo hace mas modular y lleva a que los objetos sean totalmente independientes unos de los otros. Como ejemplo podría decir que si se decide que ya los turnos y el avance no esta dado por un dado y su restado, yo podría eliminar aquel dado del juego sin tener errores de copilación. 

Para lograr esto decidí hacer uso de un singleton para el manejado de eventos y uno para el controlador del juego. Estas serian las únicas referencias obligatorias a mantener.

Otra característica del diseño es que para evitar y mejorar rendimientos evite hacer uso de los eventos de Update y de buscar objetos para obtener referencia a ellos usando el gameobject.Find(). Igualmente los objetos estáticos se marcaron como tal.

Hay un pequeño desgaste de recursos el el event manager que tengo ya que el hace “Boxing” y su correspondiente “Unboxing”. Esto esta marcado dentro del código con un //TODO: para mejorar en el futuro proximo. Igualmente hay otro mas comentarios similares.

El coding esta bien documentado. Ademas hay html con descripción de las clases y algunos diagramas. Esto esta en "Juego_Escalera/html/index.html".


