public enum DisperserState
{
	activated,					// The mesh is activated. The scene is currently showing the real mesh, without any animations.
	deactivated,				// The mesh is deactivated. The real mesh is disabled, and there are not animations.
	activating,					// The mesh is activating itself. The real mesh is temporarily disabled, and it is animating the thousand and tiny submeshes facing towards the real position of the mesh. 
	deactivating,				// The mesh is deactivating itself. The real mesh is temporarily disabled, and it is animating the thousand and tiny submeshes facing towards the oposite real position of the mesh.
	activatingFadeTransition	// (Deprecated, don't erase it please) When the thousands and tiny submeshes have to dissapear, if they are different from the real renderer, you can always make an alpha fade transition between the submeshes an the real mesh.
}